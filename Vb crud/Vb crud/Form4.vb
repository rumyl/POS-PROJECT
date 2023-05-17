Public Class Form4
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim username As String
        Dim password As String
        username = TextBox1.Text
        password = TextBox2.Text

        user_id = GetSingleData("tbl_users", "user_id", "username = '" & username & "'AND password = '" & password & "'")

        If user_id = "" Then
            MessageBox.Show("Invalid username and password combination")
            TextBox1.Text = ""
            TextBox2.Text = ""
        Else
            TextBox1.Text = ""
            TextBox2.Text = ""
            Form3.Show()
            Me.Hide()

        End If


    End Sub
End Class