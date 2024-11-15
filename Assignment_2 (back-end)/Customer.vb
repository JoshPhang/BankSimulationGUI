Public Class Customer
    Public Property id As Integer
    Public Property arrTime As Integer
    Public Property serveTime As Integer
    Public Property departTime As Integer
    Public Property waitTime As Integer
    Public Property hasArrived As Integer

    Public Sub New(arr As Integer, serve As Integer)
        arrTime = arr
        serveTime = serve
        hasArrived = False
    End Sub
End Class
