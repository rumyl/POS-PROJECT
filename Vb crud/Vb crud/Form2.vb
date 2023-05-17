Public Class Form2
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox4.TextChanged

        Dim tableName As String = "tbl_products"
        Dim columnName As String = "product_name"
        Dim columnReturn As String = "product_id, product_name, stocks"
        Dim searchText As String = TextBox1.Text

        Dim searchResult As DataTable = SearchData(tableName, searchText, columnName, columnReturn)
        DataGridView1.DataSource = searchResult


        If searchResult IsNot Nothing AndAlso searchResult.Rows.Count > 0 Then
            ' kara eon ibutang du data kung may sueod
            DataGridView1.DataSource = searchResult

            ' i disable naton du first column para di ma edit du primary key
            DataGridView1.ReadOnly = True

            ' kara ga edit it header it atong datagrid
            DataGridView1.Columns(0).HeaderText = "Product Number"
            DataGridView1.Columns(1).HeaderText = "Product Name"
            DataGridView1.Columns(2).HeaderText = "Stocks"


        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim selectedRows As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows

        If selectedRows.Count > 0 Then
            If TextBox2.Text = "" Then
                MessageBox.Show("Please enter a quantity")
            Else

                Dim id As Integer = CInt(selectedRows(0).Cells("product_id").Value)
                Dim data As New Dictionary(Of String, Object)()
                data.Add("product_id", id)
                data.Add("qty", TextBox2.Text)
                data.Add("user_id", user_id)

                ' Call naton du function para sa save
                Dim tableName As String = "tbl_temp1"
                Dim success As Boolean = MySQLConnector.InsertData(tableName, data)


                ' Dikaraya i check naton kung nag save or uwa
                If success Then
                    TextBox2.Text = ""

                    'bue on naton du data sa database gamit du GetDataTabke nga function

                    Dim query As String = "SELECT  tbl_temp1.product_id, tbl_products.product_name, SUM(tbl_temp1.qty) as qty  FROM tbl_products INNER JOIN tbl_temp1 ON tbl_products.product_id = tbl_temp1.product_id WHERE tbl_temp1.user_id ='" & user_id & "' GROUP BY tbl_products.product_id"

                    Dim dt As DataTable = GetData(query)

                    ' kara i check naton kung may sueod o wa du table
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        ' kara eon ibutang du data kung may sueod
                        DataGridView2.DataSource = dt
                        ' i disable naton du first column para di ma edit du primary key
                        DataGridView2.Columns(0).ReadOnly = True

                        ' kara ga edit it header it atong datagrid
                        DataGridView2.Columns(0).HeaderText = "Product ID"
                        DataGridView2.Columns(1).HeaderText = "Product Name"
                        DataGridView2.Columns(2).HeaderText = "Qty"

                    End If

                Else
                    MessageBox.Show("Error inserting data.")
                End If
            End If
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Form3.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox3.Text = "" Or DateTimePicker1.Text = "" Then
            MessageBox.Show("All fields are required")

        Else

            Dim selectedDate As DateTime = DateTimePicker1.Value
            Dim formattedDate As String = selectedDate.ToString("yyyy-MM-dd HH:mm:ss")
            Dim data As New Dictionary(Of String, Object)()

            data.Add("supplier", TextBox3.Text)
            data.Add("purchase_date", formattedDate)

            ' Call naton du function para sa save
            Dim tableName As String = "tbl_stockin"
            Dim success As Boolean = MySQLConnector.InsertData(tableName, data)

            ' Dikaraya i check naton kung nag save or uwa
            If success Then


                Dim stockin_id As Integer = GetLastInsertedID("tbl_stockin")
                stockin_details(user_id, stockin_id)


                Dim query As String = "SELECT  tbl_temp1.product_id, tbl_products.product_name, SUM(tbl_temp1.qty) as qty  FROM tbl_products INNER JOIN tbl_temp1 ON tbl_products.product_id = tbl_temp1.product_id WHERE tbl_temp1.user_id ='" & user_id & "' GROUP BY tbl_products.product_id"
                Dim dt As DataTable = GetData(query)
                DataGridView2.DataSource = dt

                Dim query2 As String = "SELECT  * FROM tbl_products"
                Dim dt2 As DataTable = GetData(query2)
                DataGridView1.DataSource = dt2

                MessageBox.Show("Purchase completed!")
                TextBox3.Text = ""
                DateTimePicker1.Text = ""




            Else
                MessageBox.Show("Error inserting data.")
            End If
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim selectedRows As DataGridViewSelectedRowCollection = DataGridView2.SelectedRows

        If selectedRows.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                Dim id As Integer = CInt(selectedRows(0).Cells("product_id").Value)

                Dim tableName As String = "tbl_temp1"
                Dim whereClause As String = "product_id = " & id & " AND  user_id = '" & user_id & "'"


                If DeleteData(tableName, whereClause) Then

                    Dim query As String = "SELECT  tbl_temp1.product_id, tbl_products.product_name, SUM(tbl_temp1.qty) as qty  FROM tbl_products INNER JOIN tbl_temp1 ON tbl_products.product_id = tbl_temp1.product_id WHERE tbl_temp1.user_id ='" & user_id & "' GROUP BY tbl_products.product_id"

                    Dim dt As DataTable = MySQLConnector.GetData(query)
                    DataGridView2.DataSource = dt
                Else
                    MessageBox.Show(DeleteData(tableName, whereClause))


                    MessageBox.Show("Error removing record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Else
            MessageBox.Show("Please select a row to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim tableName As String = "tbl_temp1"
        Dim whereClause As String = "user_id = '" & user_id & "'"

        DeleteData(tableName, whereClause)

    End Sub
End Class