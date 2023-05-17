Public Class Form6
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form3.Show()
        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click


        If TextBox3.Text = "" Then
            MessageBox.Show("All fields are required")

        Else

            Dim tendered As Double = TextBox3.Text
            Dim total_amount As Double = TextBox2.Text
            Dim changed As Double = TextBox4.Text
            Dim customer As String = TextBox5.Text
            Dim data As New Dictionary(Of String, Object)()

            data.Add("tendered", tendered)
            data.Add("total_amount", total_amount)
            data.Add("changed", changed)
            data.Add("customer", customer)
            data.Add("user_id", user_id)


            ' Call naton du function para sa save
            Dim tableName As String = "tbl_stockout"
            Dim success As Boolean = MySQLConnector.InsertData(tableName, data)

            ' Dikaraya i check naton kung nag save or uwa
            If success Then


                Dim stockout_id As Integer = GetLastInsertedID("tbl_stockout")
                stockout_details(user_id, stockout_id)


                Dim query As String = "SELECT  tbl_temp2.product_id, tbl_products.product_name, SUM(tbl_temp2.qty) as qty,  tbl_temp2.price  FROM tbl_products INNER JOIN tbl_temp2 ON tbl_products.product_id = tbl_temp2.product_id WHERE tbl_temp2.user_id ='" & user_id & "' GROUP BY tbl_products.product_id"
                Dim dt As DataTable = GetData(query)
                DataGridView1.DataSource = dt


                MessageBox.Show("Sales completed!")
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                TextBox5.Text = ""

                GenerateReceipt(stockout_id, user_id)

            Else
                MessageBox.Show("Error inserting data.")
            End If
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label8.Text = DateTime.Now.ToString("F")
    End Sub

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        Dim tableName As String = "tbl_temp2"
        Dim whereClause As String = "user_id = '" & user_id & "'"

        DeleteData(tableName, whereClause)

    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            Dim inputValue As String = TextBox1.Text.Trim()
            Dim product_id As String
            Dim qty As Integer
            Dim barcode As String = ""
            Dim product_price As Double
            If inputValue.Contains("*") Then
                Dim parts As String() = inputValue.Split("*"c)
                If parts.Length >= 2 Then
                    qty = parts(0).Trim()
                    barcode = parts(1).Trim()


                    e.Handled = True
                End If

            Else
                qty = 1
                barcode = TextBox1.Text

            End If

            product_id = GetSingleData("tbl_products", "product_id", "stocks > 0 AND barcode = '" & barcode & "'")
            If product_id <> "" Then
                product_price = GetSingleData("tbl_products", "srp", "product_id = '" & product_id & "'")


                Dim data As New Dictionary(Of String, Object)()
                data.Add("product_id", product_id)
                data.Add("qty", qty)
                data.Add("price", product_price)
                data.Add("user_id", user_id)

                ' Call naton du function para sa save
                Dim tableName As String = "tbl_temp2"
                Dim success As Boolean = MySQLConnector.InsertData(tableName, data)


                ' Dikaraya i check naton kung nag save or uwa
                If success Then
                    TextBox1.Text = ""

                    'bue on naton du data sa database gamit du GetDataTabke nga function


                    Dim total_amount As Double = GetSingleData("tbl_temp2", " SUM(qty * price) AS total_amounts", "user_id='" & user_id & "' GROUP BY user_id")
                    TextBox2.Text = total_amount
                    TextBox3.Text = ""
                    TextBox4.Text = ""

                    Dim query As String = "SELECT  tbl_temp2.product_id, tbl_products.product_name, SUM(tbl_temp2.qty) as qty , tbl_temp2.price  FROM tbl_products INNER JOIN tbl_temp2 ON tbl_products.product_id = tbl_temp2.product_id WHERE tbl_temp2.user_id ='" & user_id & "' GROUP BY tbl_products.product_id"

                    Dim dt As DataTable = GetData(query)

                    ' kara i check naton kung may sueod o wa du table
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        ' kara eon ibutang du data kung may sueod
                        DataGridView1.DataSource = dt
                        ' i disable naton du first column para di ma edit du primary key
                        DataGridView1.Columns(0).ReadOnly = True

                        ' kara ga edit it header it atong datagrid
                        DataGridView1.Columns(0).HeaderText = "Product ID"
                        DataGridView1.Columns(1).HeaderText = "Product Name"
                        DataGridView1.Columns(2).HeaderText = "Qty"
                        DataGridView1.Columns(3).HeaderText = "Price"

                    End If

                Else
                    MessageBox.Show("Error inserting data.")
                End If
            Else
                TextBox1.Text = ""
                MessageBox.Show("No Product found or Out of Stocks!")
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim selectedRows As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows

        If selectedRows.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                Dim id As Integer = CInt(selectedRows(0).Cells("product_id").Value)

                Dim tableName As String = "tbl_temp2"
                Dim whereClause As String = "product_id = " & id

                If DeleteData(tableName, whereClause) Then
                    MessageBox.Show("Item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Dim query As String = "SELECT  tbl_temp2.product_id, tbl_products.product_name, SUM(tbl_temp2.qty) as qty , tbl_temp2.price  FROM tbl_products INNER JOIN tbl_temp2 ON tbl_products.product_id = tbl_temp2.product_id WHERE tbl_temp2.user_id ='" & user_id & "' GROUP BY tbl_products.product_id"
                    Dim dt As DataTable = MySQLConnector.GetData(query)
                    DataGridView1.DataSource = dt
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        Dim total_amount As Double = GetSingleData("tbl_temp2", " SUM(qty * price) AS total_amounts", "user_id='" & user_id & "' GROUP BY user_id")

                        TextBox2.Text = total_amount
                        TextBox3.Text = ""
                        TextBox4.Text = ""
                    Else
                        'Dim total_amount As Double = GetSingleData("tbl_temp2", " SUM(qty * price) AS total_amounts", "user_id='" & user_id & "' GROUP BY user_id")
                        TextBox2.Text = ""
                        TextBox3.Text = ""
                        TextBox4.Text = ""
                    End If
                End If
            End If
        Else
            MessageBox.Show("Please select a row to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        TextBox4.Text = Val(TextBox3.Text) - Val(TextBox2.Text)
    End Sub
End Class