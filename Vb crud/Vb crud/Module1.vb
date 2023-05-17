Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing
Imports System.Drawing

Public Module Module1
    Public user_id As string
End Module

Module MySQLConnector
    Private ReadOnly connectionString As String = "server=localhost;database=sample_db;uid=root;password=;"

    Public Function GetData(ByVal selectCommand As String) As DataTable
        Dim dataTable As New DataTable()

        Using connection As New MySqlConnection(connectionString)
            connection.Open()

            Using adapter As New MySqlDataAdapter(selectCommand, connection)
                adapter.Fill(dataTable)
            End Using

            connection.Close()
        End Using

        Return dataTable
    End Function


    Public Function SearchData(ByVal tableName As String, ByVal search As String, ByVal searchColumn As String, ByVal returnColumn As String) As DataTable
        Dim dt As New DataTable()
        Using connection As New MySqlConnection(connectionString)
            Dim query As String = "SELECT " & returnColumn & " FROM " & tableName & " WHERE " & searchColumn & " LIKE @search LIMIT 10"
            Dim cmd As New MySqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@search", "%" & search & "%")
            Dim adapter As New MySqlDataAdapter(cmd)
            adapter.Fill(dt)
        End Using

        Return dt
    End Function


    Public Function GetSingleData(tableName As String, columnName As String, whereClause As String) As String
        Dim query As String = $"SELECT {columnName} FROM {tableName} WHERE {whereClause};"
        Dim result As String = ""

        Using connection As New MySqlConnection(connectionString)
            Using command As New MySqlCommand(query, connection)
                Try
                    connection.Open()
                    Dim reader As MySqlDataReader = command.ExecuteReader()

                    If reader.Read() Then
                        result = reader(0).ToString()
                    End If

                    reader.Close()
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using
        End Using

        Return result

        ' Dim value As String = GetSingleData("mytable", "mycolumn", "id=1")
        ' MessageBox.Show(value)

    End Function



    Public Sub GenerateReceipt(ByVal stockout_id As Integer, ByVal user_id As String)
        Dim query As String = "SELECT * FROM tbl_stockout WHERE stockout_id = @stockout_id"

        Using connection As New MySqlConnection(connectionString)
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@stockout_id", stockout_id)
                connection.Open()
                Dim reader As MySqlDataReader = command.ExecuteReader()

                If reader.HasRows Then

                    Dim customerName As String = GetSingleData("tbl_stockout", "customer", "stockout_id = '" & stockout_id & "'")
                    Dim totalAmount As Decimal = GetSingleData("tbl_stockout", "total_amount", "stockout_id = '" & stockout_id & "'")
                    Dim cashier As String = GetSingleData("tbl_users", "fullname", "user_id = '" & user_id & "'")


                    PrintReceipt(stockout_id, customerName, totalAmount, cashier)
                Else
                    Console.WriteLine("No receipt found with ID: " & stockout_id)
                End If

                reader.Close()
            End Using
        End Using
    End Sub

    Public Sub PrintReceipt(ByVal id As Integer, ByVal customerName As String, ByVal totalAmount As Decimal, ByVal cashierName As String)
        Dim pd As New PrintDocument()
        AddHandler pd.PrintPage, Sub(sender As Object, e As PrintPageEventArgs)
                                     ' Define the font and brush for printing
                                     Dim font As New Font("Merchant Copy", 10.5)
                                     Dim brush As New SolidBrush(Color.Black)

                                     ' Define the left margin value
                                     Dim leftMargin As Integer = 0

                                     ' Define the receipt content
                                     Dim receiptContent As String =
                                                       "              ABC COMPANY       " & vbCrLf &
                                                       "             KALIBO, AKLAN      " & vbCrLf &
                                                       "                123456          " & vbCrLf &
                                                       "-------------------------------------" & vbCrLf &
                                                       "Receipt ID: " & id & vbCrLf &
                                                       "Customer Name: " & customerName & vbCrLf &
                                                       "Cashier: " & cashierName & vbCrLf &
                                                       "-------------------------------------" & vbCrLf &
                                                       "Item             Qty Price Subtotal" & vbCrLf &
                                                       "-------------------------------------" & vbCrLf

                                     Dim yPos As Single = 200

                                     ' Retrieve item details from the database
                                     ' Assuming there is a table named 'items' with columns 'name', 'quantity', 'price', etc.
                                     Dim itemsQuery As String = "SELECT product_id, qty, user_id, price FROM tbl_stockout_details WHERE stockout_id = @stockout_id"

                                     Using connection As New MySqlConnection(connectionString)
                                         Using command As New MySqlCommand(itemsQuery, connection)
                                             command.Parameters.AddWithValue("@stockout_id", id)
                                             connection.Open()
                                             Dim reader As MySqlDataReader = command.ExecuteReader()

                                             While reader.Read()

                                                 Dim product_id As Integer = reader.GetString(reader.GetOrdinal("product_id"))
                                                 Dim itemName As String = GetSingleData("tbl_products", "product_name", "product_id = '" & product_id & "'")
                                                 Dim quantity As Integer = reader.GetInt32(reader.GetOrdinal("qty"))
                                                 Dim price As Decimal = reader.GetDecimal(reader.GetOrdinal("price"))
                                                 Dim subtotal As Decimal = quantity * price
                                                 receiptContent += $"{itemName,-18} {quantity,-4} {price,-5} {subtotal.ToString("N")}" & vbCrLf


                                                 yPos += 20  ' Adjust the spacing here

                                                 ' Check if there is enough space left for the overall total
                                                 If yPos >= e.MarginBounds.Bottom - 40 Then
                                                     ' If not, indicate that there are more pages to print
                                                     e.HasMorePages = True
                                                     reader.Close()
                                                     Return
                                                 End If
                                             End While

                                             reader.Close()
                                         End Using

                                         ' Draw the receipt content within the rectangle
                                         e.Graphics.DrawString(receiptContent, font, brush, leftMargin, 50)

                                         ' Print the overall total
                                         Dim overallTotalText As String = "-------------------------------------" & vbCrLf &
                                                           "Total Amount: " & totalAmount.ToString("N")
                                         e.Graphics.DrawString(overallTotalText, font, brush, leftMargin, yPos + 20)

                                         ' Dispose of the font and brush objects
                                         font.Dispose()
                                         brush.Dispose()

                                         ' Indicate that there are no more pages to print
                                         e.HasMorePages = False


                                     End Using
                                 End Sub

        ' Print the receipt using the default printer
        pd.Print()
    End Sub




    Public Function GetLastInsertedID(ByVal tableName As String) As Integer
        Dim lastInsertedID As Integer = 0

        ' Create connection and command objects
        Using connection As New MySqlConnection(connectionString)
            Using command As New MySqlCommand()
                command.Connection = connection

                ' Set the SQL query to retrieve the last inserted ID for the specified table
                command.CommandText = $"SELECT LAST_INSERT_ID() FROM {tableName};"

                Try
                    connection.Open()

                    ' Execute the query and retrieve the last inserted ID
                    lastInsertedID = Convert.ToInt32(command.ExecuteScalar())
                Catch ex As Exception
                    ' Handle any exceptions here
                    Console.WriteLine("An error occurred: " + ex.Message)
                End Try
            End Using
        End Using

        Return lastInsertedID
    End Function

    Public Function CheckRecordExists(ByVal tableName As String, ByVal columnName As String, ByVal columnValue As String) As Boolean
        Dim recordExists As Boolean = False
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Dim query As String = $"SELECT COUNT(*) FROM {tableName} WHERE {columnName} = @Value"
                Dim command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@Value", columnValue)

                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                If count > 0 Then
                    recordExists = True
                End If

            End Using
        Catch ex As Exception
            Console.WriteLine("Error checking record existence: " & ex.Message)
        End Try

        Return recordExists
    End Function

    Public Function stockin_details(ByVal id As String, stockin_id As String) As Boolean

        Dim tablefrom As String = "tbl_temp1"
        Dim tableto As String = "tbl_stockin_details"


        ' Create connection and command objects
        Using connection As New MySqlConnection(connectionString)
            Using command As New MySqlCommand()
                command.Connection = connection
                connection.Open()

                ' Select records with the specified ID from the source table
                Dim selectQuery As String = $"SELECT product_id, SUM(qty) as total FROM {tablefrom} WHERE user_id = @id GROUP by product_id"
                command.CommandText = selectQuery
                command.Parameters.AddWithValue("@id", id)
                ' Retrieve the records
                Dim dataTable As New DataTable()
                    Dim adapter As New MySqlDataAdapter(command)
                    adapter.Fill(dataTable)

                    ' Insert the records into the destination table
                    If dataTable.Rows.Count > 0 Then
                    ' Create the insert command
                    Dim insertQuery As String = $"INSERT INTO {tableto} (stockin_id, product_id, qty, user_id) VALUES (@stockin_id, @product_id, @qty, @user_id)"
                    command.CommandText = insertQuery

                    ' Set up the parameters
                    command.Parameters.Add("@stockin_id", SqlDbType.Int)
                    command.Parameters.Add("@product_id", SqlDbType.Int)
                    command.Parameters.Add("@qty", SqlDbType.Int)
                    command.Parameters.Add("@user_id", SqlDbType.Int)

                    ' Iterate through the records and insert them
                    For Each row As DataRow In dataTable.Rows

                        Dim productId As Integer = CInt(row("product_id"))
                        Dim qty As Integer = CInt(row("total"))


                        'kara ga update it stocks
                        Dim stocks As Integer = GetSingleData("tbl_products", "stocks", "product_id=" & productId)
                        Dim data As New Dictionary(Of String, Object)()
                        Dim condition As String = "WHERE product_id = '" & productId & "'"
                        data.Add("stocks", stocks + qty)
                        UpdateData("tbl_products", data, condition)

                        Dim whereClause As String = "product_id = " & productId & " AND user_id ='" & id & "'"
                        DeleteData("tbl_temp1", whereClause)

                        command.Parameters("@stockin_id").Value = stockin_id
                        command.Parameters("@product_id").Value = productId
                        command.Parameters("@qty").Value = qty
                        command.Parameters("@user_id").Value = user_id

                        command.ExecuteNonQuery()
                    Next
                    End If

                    Return True

            End Using
        End Using
    End Function


    Public Function stockout_details(ByVal id As String, stockout_id As String) As Boolean

        Dim tablefrom As String = "tbl_temp2"
        Dim tableto As String = "tbl_stockout_details"


        ' Create connection and command objects
        Using connection As New MySqlConnection(connectionString)
            Using command As New MySqlCommand()
                command.Connection = connection
                connection.Open()

                ' Select records with the specified ID from the source table
                Dim selectQuery As String = $"SELECT product_id, SUM(qty) as total, price  FROM {tablefrom} WHERE user_id = @id GROUP by product_id"
                command.CommandText = selectQuery
                command.Parameters.AddWithValue("@id", id)
                ' Retrieve the records
                Dim dataTable As New DataTable()
                Dim adapter As New MySqlDataAdapter(command)
                adapter.Fill(dataTable)

                ' Insert the records into the destination table
                If dataTable.Rows.Count > 0 Then
                    ' Create the insert command
                    Dim insertQuery As String = $"INSERT INTO {tableto} (stockout_id, product_id, qty, price, user_id) VALUES (@stockout_id, @product_id, @qty, @price, @user_id)"
                    command.CommandText = insertQuery

                    ' Set up the parameters
                    command.Parameters.Add("@stockout_id", SqlDbType.Int)
                    command.Parameters.Add("@product_id", SqlDbType.Int)
                    command.Parameters.Add("@qty", SqlDbType.Int)
                    command.Parameters.Add("@price", SqlDbType.Int)
                    command.Parameters.Add("@user_id", SqlDbType.Int)

                    ' Iterate through the records and insert them
                    For Each row As DataRow In dataTable.Rows

                        Dim productId As Integer = CInt(row("product_id"))
                        Dim qty As Double = CInt(row("total"))
                        Dim price As Integer = CInt(row("price"))


                        'kara ga update it stocks
                        Dim stocks As Integer = GetSingleData("tbl_products", "stocks", "product_id=" & productId)
                        Dim data As New Dictionary(Of String, Object)()
                        Dim condition As String = "WHERE product_id = '" & productId & "'"
                        data.Add("stocks", stocks - qty)
                        UpdateData("tbl_products", data, condition)

                        Dim whereClause As String = "product_id = " & productId & " AND user_id ='" & id & "'"
                        DeleteData("tbl_temp2", whereClause)

                        command.Parameters("@stockout_id").Value = stockout_id
                        command.Parameters("@product_id").Value = productId
                        command.Parameters("@qty").Value = qty
                        command.Parameters("@price").Value = price
                        command.Parameters("@user_id").Value = user_id

                        command.ExecuteNonQuery()
                    Next
                End If

                Return True

            End Using
        End Using
    End Function

    Public Function InsertData(tableName As String, data As Dictionary(Of String, Object)) As Boolean
        Dim query As String = String.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, String.Join(",", data.Keys), String.Join(",", data.Values.Select(Function(value) String.Format("'{0}'", value))))
        Using connection As New MySqlConnection(connectionString)
            Dim command As New MySqlCommand(query, connection)
            connection.Open()
            Dim rowsAffected As Integer = command.ExecuteNonQuery()
            Return rowsAffected > 0
        End Using
    End Function

    Public Function UpdateData(ByVal tableName As String, ByVal data As Dictionary(Of String, Object), ByVal condition As String) As Boolean
        ' kara naton ayuson du where clause
        Dim setClause As String = String.Join(", ", data.Select(Function(x) String.Format("{0} = '{1}'", x.Key, x.Value)))

        ' pag katapos ma ayos du where clause buuon eon naton du query
        Dim query As String = String.Format("UPDATE {0} SET {1} {2}", tableName, setClause, condition)

        ' ikonek naton sa database
        Using connection As New MySqlConnection(connectionString)
            connection.Open()

            ' kara eon kita ma command para sa update
            Using command As New MySqlCommand(query, connection)
                ' i execute eon ru command
                Dim result As Integer = command.ExecuteNonQuery()

                ' i return naton du true kung nag update na sa database
                Return result > 0
            End Using
        End Using
    End Function

    Public Function DeleteData(tableName As String, whereClause As String) As Boolean
        Dim success As Boolean = False

        Dim query As String = $"DELETE FROM {tableName} WHERE {whereClause}"

        Using connection As New MySqlConnection(connectionString)
            Using command As New MySqlCommand(query, connection)
                Try
                    connection.Open()
                    Dim rowsAffected As Integer = command.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        success = True
                    End If
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            End Using
        End Using

        Return success
    End Function

    'additional code





End Module
