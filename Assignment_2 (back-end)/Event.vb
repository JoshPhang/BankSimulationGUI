Public Class [Event]
    Public Property event_id As Integer
    Public Property time As Integer

    Public Sub New(newID As Integer, newTime As Integer)
        event_id = newID
        time = newTime
    End Sub
End Class
