Imports System.Text.RegularExpressions
Public Class Form6
    Dim quantity As Integer = 1
    Dim menu_id As Integer
    Private Sub Textbox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim regex As New Regex("^\d*$") ' Regular expression to match numeric input

        If Not regex.IsMatch(TextBox2.Text) Then
            ' Remove non-numeric characters from the TextBox
            TextBox2.Text = Regex.Replace(TextBox2.Text, "[^\d]", "")
        End If

        ' Update TextBox text with the new quantity
        TextBox2.Text = quantity.ToString()
    End Sub

    Private Sub TextBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Up Then
            ' Increase quantity
            quantity += 1
        ElseIf e.KeyCode = Keys.Down Then
            ' Decrease quantity (minimum value is 1)
            If quantity > 1 Then
                quantity -= 1
            End If
        End If

        ' Update TextBox text with the new quantity
        TextBox2.Text = quantity.ToString()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Form3.Show()
    End Sub

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim tableName As String = "tbl_temp"
        Dim whereClause As String = " user_id='" & user_id & "'"

        DeleteData(tableName, whereClause)
        loadButton("tbl_category", Me)


    End Sub

    Public Sub ButtonCat_Click(sender As Object, e As EventArgs)
        Dim clickedButton As Button = CType(sender, Button)
        Dim buttonId As Integer = Integer.Parse(clickedButton.Name.Replace("Button", ""))

        loadButton2("tbl_menu", buttonId, Me)
    End Sub

    Public Sub ButtonMenu_Click(sender As Object, e As EventArgs)
        Dim clickedButton As Button = CType(sender, Button)
        Dim buttonId As Integer = Integer.Parse(clickedButton.Name.Replace("Button", ""))
        menu_id = buttonId
        'MessageBox.Show(buttonId)
        TextBox1.Text = GetSingleData("tbl_menu", "menu_name", "menu_id =" & buttonId)
        TextBox2.Text = 0
        TextBox2.Select()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        If TextBox1.Text = "" Or TextBox2.Text = "" Then
            MessageBox.Show("Select menu first.")

        Else

            Dim data As New Dictionary(Of String, Object)()

            Dim price As Double = GetSingleData("tbl_menu", "price", "menu_id =" & menu_id)

            data.Add("menu_id", menu_id)
            data.Add("qty", TextBox2.Text)
            data.Add("price", price)
            data.Add("user_id", user_id)


            ' Call naton du function para sa save
            Dim tableName As String = "tbl_temp"
            Dim success As Boolean = MySQLConnector.InsertData(tableName, data)



            ' Dikaraya i check naton kung nag save or uwa
            If success Then

                TextBox1.Text = ""
                TextBox2.Text = ""

                'bue on naton du data sa database gamit du GetDataTable nga function
                Dim query As String = "SELECT  tbl_temp.menu_id, tbl_menu.menu_name, SUM(tbl_temp.qty) as qty  FROM tbl_menu INNER JOIN tbl_temp ON tbl_menu.menu_id = tbl_temp.menu_id WHERE tbl_temp.user_id ='" & user_id & "' GROUP BY tbl_menu.menu_id"

                Dim dt As DataTable = GetData(query)

                ' kara i check naton kung may sueod o wa du table
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    ' kara eon ibutang du data kung may sueod
                    DataGridView1.DataSource = dt
                    ' i disable naton du first column para di ma edit du primary key
                    DataGridView1.Columns(0).ReadOnly = True

                    ' kara ga edit it header it atong datagrid
                    DataGridView1.Columns(0).HeaderText = "Menu ID"
                    DataGridView1.Columns(1).HeaderText = "Menu Name"
                    DataGridView1.Columns(2).HeaderText = "Qty"

                End If

            Else
                MessageBox.Show("Error inserting data.")
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim selectedRows As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows

        If selectedRows.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                Dim id As Integer = CInt(selectedRows(0).Cells("menu_id").Value)

                Dim tableName As String = "tbl_temp"
                Dim whereClause As String = "menu_id = " & id & " AND user_id='" & user_id & "'"

                If DeleteData(tableName, whereClause) Then
                    MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Dim query As String = "SELECT  tbl_temp.menu_id, tbl_menu.menu_name, SUM(tbl_temp.qty) as qty  FROM tbl_menu INNER JOIN tbl_temp ON tbl_menu.menu_id = tbl_temp.menu_id WHERE tbl_temp.user_id ='" & user_id & "' GROUP BY tbl_menu.menu_id"

                    Dim dt As DataTable = MySQLConnector.GetData(query)
                    DataGridView1.DataSource = dt

                Else
                    MessageBox.Show(DeleteData(tableName, whereClause))


                    MessageBox.Show("Error deleting record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Else
            MessageBox.Show("Please select a row to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click


        If TextBox3.Text = "" Then
            MessageBox.Show("All fields are required")

        Else

            Dim tendered As Double = TextBox4.Text
            Dim total_amount As Double = TextBox3.Text
            Dim changed As Double = TextBox5.Text
            Dim customer As String = TextBox6.Text
            Dim data As New Dictionary(Of String, Object)()

            data.Add("tendered", tendered)
            data.Add("total_amount", total_amount)
            data.Add("changed", changed)
            data.Add("customer", customer)
            data.Add("user_id", user_id)


            ' Call naton du function para sa save
            Dim tableName As String = "tbl_sales"
            Dim success As Boolean = MySQLConnector.InsertData(tableName, data)

            ' Dikaraya i check naton kung nag save or uwa
            If success Then


                Dim sales_id As Integer = GetLastInsertedID("tbl_sales")
                sales_details(user_id, sales_id)


                Dim query As String = "SELECT  tbl_temp.menu_id, tbl_menu.menu_name, SUM(tbl_temp.qty) as qty,  tbl_temp.price  FROM tbl_menu INNER JOIN tbl_temp ON tbl_menu.menu_id = tbl_temp.menu_id WHERE tbl_temp.user_id ='" & user_id & "' GROUP BY tbl_menu.menu_id"
                Dim dt As DataTable = GetData(query)
                DataGridView1.DataSource = dt


                MessageBox.Show("Sales completed!")
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                TextBox5.Text = ""

                GenerateReceipt(sales_id, user_id)

            Else
                MessageBox.Show("Error inserting data.")
            End If
        End If
    End Sub
End Class