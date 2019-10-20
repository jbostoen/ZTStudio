
Public Class ZTStudioException
    Inherits System.ApplicationException

    Dim StrException_Class As String = ""
    Dim StrException_Method As String = ""
    Dim ObjException_ErrObject As ErrObject = Nothing

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

    Public Property ErrObject As ErrObject
        Get
            Return ObjException_ErrObject

        End Get
        Set(value As ErrObject)
            ObjException_ErrObject = value
        End Set
    End Property

    Public Sub New(ByVal StrClass As String, ByVal StrMethod As String, ByVal ObjError As ErrObject)

        MyBase.New(StrClass & "::" & StrMethod & "() - " & ObjError.Number & " - " & ObjError.Description & " at line " & ObjError.Erl)


        Me.ClassName = StrClass
        Me.MethodName = StrMethod
        Me.ErrObject = ObjError

    End Sub

End Class