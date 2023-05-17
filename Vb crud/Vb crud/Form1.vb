Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim check As Boolean = CheckRecordExists("tbl_products", "barcode", TextBox2.Text)

        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Then
            MessageBox.Show("All fields are required")

        ElseIf check = True Then

            MessageBox.Show("Barcode already exist!")

        Else

            Dim data As New Dictionary(Of String, Object)()
            data.Add("product_name", TextBox1.Text)
            data.Add("barcode", TextBox2.Text)
            data.Add("cost", TextBox3.Text)
            data.Add("srp", TextBox4.Text)
            data.Add("stocks", TextBox5.Text)

            ' Call naton du function para sa save
            Dim tableName As String = "tbl_products"
            Dim success As Boolean = MySQLConnector.InsertData(tableName, data)

            ' Dikaraya i check naton kung nag save or uwa
            If success Then
                MessageBox.Show("Data inserted successfully.")
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                TextBox5.Text = ""

                'bue on naton du data sa database gamit du GetDataTable nga function
                Dim query As String = "SELECT * FROM tbl_products"
                Dim dt As DataTable = MySQLConnector.GetData(query)

                ' kara i check naton kung may sueod o wa du table
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    ' kara eon ibutang du data kung may sueod
                    DataGridView1.DataSource = dt
                End If

            Else
                MessageBox.Show("Error inserting data.")
            End If
        End If
    End Sub

    Private Sub dataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        'kara naton bue-on du updated eon nga data
        Dim newValue As String = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()
        Dim columnName As String = DataGridView1.Columns(e.ColumnIndex).Name

        ' bue on ta ru primary key it gin edit naton nga data
        Dim primaryKey As String = DataGridView1.Rows(e.RowIndex).Cells("product_id").Value.ToString()

        ' isueod naton sa array du mga data
        Dim data As New Dictionary(Of String, Object)()
        data.Add(columnName, newValue)

        'i specify naton du connection nga need para sa pag haboy naton sa function naton nga Update
        Dim condition As String = "WHERE product_id = '" & primaryKey & "'"

        'i call eon naton du function dikaraya
        Dim success As Boolean = UpdateData("tbl_products", data, condition)

        ' check if success or not
        If success Then
            MessageBox.Show("Record updated successfully!")
        Else
            MessageBox.Show("Failed to update record!")
        End If
    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim query As String = "SELECT * FROM tbl_products"
        Dim dataTable As DataTable = GetData(query)
        DataGridView1.DataSource = dataTable

        ' kara i check naton kung may sueod o wa du table
        If dataTable IsNot Nothing AndAlso dataTable.Rows.Count > 0 Then
            ' kara eon ibutang du data dataTable may sueod

            ' i disable naton du first column para di ma edit du primary key
            DataGridView1.Columns(0).ReadOnly = True

            ' kara ga edit it header it atong datagrid
            DataGridView1.Columns(0).HeaderText = "Product Number"
            DataGridView1.Columns(1).HeaderText = "Product Name"
            DataGridView1.Columns(2).HeaderText = "Barcode"
            DataGridView1.Columns(3).HeaderText = "Cost"
            DataGridView1.Columns(4).HeaderText = "Selling Price"
            DataGridView1.Columns(5).HeaderText = "Stocks"

        End If




    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim selectedRows As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows

        If selectedRows.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                Dim id As Integer = CInt(selectedRows(0).Cells("product_id").Value)

                Dim tableName As String = "tbl_products"
                Dim whereClause As String = "product_id = " & id

                If DeleteData(tableName, whereClause) Then
                    MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Dim query As String = "SELECT * FROM tbl_products"
                    Dim dt As DataTable = MySQLConnector.GetData(query)

                    ' kara i check naton kung may sueod o wa du table
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        ' kara eon ibutang du data kung may sueod
                        DataGridView1.DataSource = dt
                    End If
                Else
                    MessageBox.Show(DeleteData(tableName, whereClause))


                    MessageBox.Show("Error deleting record", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Else
            MessageBox.Show("Please select a row to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub



    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
        Form3.Show()
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click, Label7.Click

    End Sub
End Class
