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

        ' Builds the reversed, space-separated byte-pair string directly instead of via
        ' Enumerable.Range(...).Select(...).ToList() + Reverse() + Join - this is called very
        ' frequently throughout multi-byte value encoding/decoding, and the LINQ chain allocated
        ' an intermediate List(Of String) and a delegate on every call for what is just "walk the
        ' string backwards two characters at a time".
        Dim SbResult As New Text.StringBuilder()

        For IntIndex As Integer = StrInput.Length - 2 To 0 Step -2
            If SbResult.Length > 0 Then
                SbResult.Append(" "c)
            End If
            SbResult.Append(StrInput, IntIndex, 2)
        Next

        Return SbResult.ToString()

    End Function

End Module