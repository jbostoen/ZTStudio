
Public Class ZTStudioException
    Inherits System.ApplicationException

    Dim StrException_Class As String = ""
    Dim StrException_Method As String = ""

    Public Property ClassName As String
        Get
            Return StrException_Class

        End Get
        Set(value As String)
            StrException_Class = value
        End Set
    End Property

    Public Property MethodName As String
        Get
            Return StrException_Method

        End Get
        Set(value As String)
            StrException_Method = value
        End Set
    End Property

    Public Sub New(ByVal StrClass As String, ByVal StrMethod As String, ByVal ObjError As Exception)

        MyBase.New(StrClass & "::" & StrMethod & "() - " & ObjError.Message, ObjError)

        Me.ClassName = StrClass
        Me.MethodName = StrMethod

    End Sub

End Class
