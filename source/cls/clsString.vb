Imports System.Runtime.CompilerServices

Module StringExtensions

    ''' <summary>
    ''' Reverse hex method to allow for easier switching around of bytes
    ''' </summary>
    ''' <remarks>In computing, endianness refers to the order of bytes (or sometimes bits) within a binary representation of a number</remarks>
    ''' <param name="StrInput">String - bytes/hex values to reverse</param>
    ''' <returns></returns>
    <Extension()>
    Public Function ReverseHex(ByVal StrInput As String) As String

        Dim StrReturn As String

        Dim LstStrings As New List(Of String)
        LstStrings.AddRange(Enumerable.Range(0, StrInput.Length / 2).Select(Function(x) StrInput.Substring(x * 2, 2)).ToList())
        LstStrings.Reverse()

        StrReturn = Strings.Join(LstStrings.ToArray(), " ")

        ' return reverse, with spaces
        Return Strings.Trim(StrReturn)

    End Function

End Module