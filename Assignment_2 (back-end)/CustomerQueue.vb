Public Class CustomerQueue
    Public Property Queue() As ArrayList

    Public Function isEmpty() As Boolean
        If Queue.Count = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub push(newCustomer As Customer)
        Queue.Add(newCustomer)
    End Sub

    Public Function pop() As Customer
        Dim item = Queue.Item(0)
        Queue.RemoveAt(0)
        Return item
    End Function

    Public Function peek() As Customer
        Dim best_index = 0
        For i As Integer = 0 To Queue.Count - 1
            If Queue(i).arrTime < Queue(best_index).arrTime Then
                best_index = i
            End If
        Next i
        Return Queue(best_index)
    End Function

    Public Sub New()
        Queue = New ArrayList
    End Sub

End Class
