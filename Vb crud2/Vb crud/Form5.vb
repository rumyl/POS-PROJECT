Public Class Form5
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
            MessageBox.Show("All fields are required")


        ElseIf TextBox3.Text <> TextBox4.Text Then
            MessageBox.Show("Password not match!")

        Else


            Dim data As New Dictionary(Of String, Object)()
            data.Add("fullname", TextBox1.Text)
            data.Add("username", TextBox2.Text)
            data.Add("password", TextBox3.Text)

            ' Call naton du function para sa save
            Dim tableName As String = "tbl_users" 'raya hay table nga na insertan
            Dim success As Boolean = MySQLConnector.InsertData(tableName, data)

            ' Dikaraya i check naton kung nag save or uwa
            If success Then
                MessageBox.Show("Data inserted successfully.")
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""


                'bue on naton du data sa database gamit du GetDataTable nga function
                Dim query As String = "SELECT user_id, fullname,  username FROM tbl_users"
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
        Dim primaryKey As String = DataGridView1.Rows(e.RowIndex).Cells("user_id").Value.ToString()

        ' isueod naton sa array du mga data
        Dim data As New Dictionary(Of String, Object)()
        data.Add(columnName, newValue)

        'i specify naton du connection nga need para sa pag haboy naton sa function naton nga Update
        Dim condition As String = "WHERE user_id = '" & primaryKey & "'"

        'i call eon naton du function dikaraya
        Dim success As Boolean = MySQLConnector.UpdateData("tbl_users", data, condition)

        ' check if success or not
        If success Then
            MessageBox.Show("Record updated successfully!")
        Else
            MessageBox.Show("Failed to update record!")
        End If
    End Sub

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim query As String = "SELECT user_id, fullname, username FROM tbl_users"
        Dim dataTable As DataTable = GetData(query)
        DataGridView1.DataSource = dataTable

        ' kara i check naton kung may sueod o wa du table
        If dataTable IsNot Nothing AndAlso dataTable.Rows.Count > 0 Then
            ' kara eon ibutang du data dataTable may sueod

            ' i disable naton du first column para di ma edit du primary key
            DataGridView1.Columns(0).ReadOnly = True

            ' kara ga edit it header it atong datagrid
            DataGridView1.Columns(0).HeaderText = "User Id"
            DataGridView1.Columns(1).HeaderText = "Fullname"
            DataGridView1.Columns(2).HeaderText = "User Name"


        End If


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim selectedRows As DataGridViewSelectedRowCollection = DataGridView1.SelectedRows

        If selectedRows.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                Dim id As Integer = CInt(selectedRows(0).Cells("user_id").Value)

                Dim tableName As String = "tbl_users"
                Dim whereClause As String = "user_id = " & id

                If DeleteData(tableName, whereClause) Then
                    MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Dim query As String = "SELECT user_id, fullname, username FROM tbl_users"
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

    Private Sub Label6_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs)

    End Sub
End Class