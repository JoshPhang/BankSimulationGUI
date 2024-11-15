Public Class EventQueue
    Public Property Queue() As ArrayList

    Public Function isEmpty() As Boolean
        If Queue.Count = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub push(newEvent As [Event])
        Queue.Add(newEvent)
    End Sub

    Public Function pop() As [Event]
        Dim best_index = 0
        For i As Integer = 0 To Queue.Count - 1
            If Queue(i).time < Queue(best_index).time Then
                best_index = i
            End If
        Next i
        Dim item = Queue(best_index)
        Queue.RemoveAt(best_index)
        Return item
    End Function

    Public Sub printQueue()
        If isEmpty() Then
            Return
        End If
        For i As Integer = 0 To Queue.Count - 1
            Console.WriteLine($"Time: {Queue(i).time} with id {Queue(i).event_id}")
        Next
    End Sub

    Public Sub New()
        Queue = New ArrayList
    End Sub

End Class
