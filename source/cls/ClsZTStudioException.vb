
''' <summary>
''' Common base for every exception ZT Studio raises internally (via MdlZTStudio.HandledError/UnhandledError).
''' Callers that only care "did something go wrong somewhere in ZT Studio" can still catch this base type;
''' callers that need to react differently depending on what kind of failure it was (e.g. skip a malformed
''' file but treat a broken settings.cfg as fatal) can catch one of the more specific subclasses below instead.
''' </summary>
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

''' <summary>
''' Raised for failures parsing/writing a ZT1 file format: a ZT1 Graphic, a .pal color palette,
''' a .ani file, or a .gpl/.PNG palette import. Typically means the input file is malformed,
''' truncated, or simply not the format it was expected to be - not a ZT Studio bug.
''' </summary>
Public Class ZTStudioFileFormatException
    Inherits ZTStudioException

    Public Sub New(ByVal StrClass As String, ByVal StrMethod As String, ByVal ObjError As Exception)
        MyBase.New(StrClass, StrMethod, ObjError)
    End Sub

End Class

''' <summary>
''' Raised for failures loading/writing ZT Studio's own configuration (settings.cfg) or other
''' application-level settings - as opposed to a ZT1 graphic/palette/animation file supplied by the user.
''' </summary>
Public Class ZTStudioConfigException
    Inherits ZTStudioException

    Public Sub New(ByVal StrClass As String, ByVal StrMethod As String, ByVal ObjError As Exception)
        MyBase.New(StrClass, StrMethod, ObjError)
    End Sub

End Class
