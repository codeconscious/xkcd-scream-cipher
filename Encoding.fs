module Encoding

open System
open System.Globalization
open Utilities

let private encodingPairs =
    [ ("A", "A"); ("B", "Ȧ"); ("C", "A̧"); ("D", "A̠"); ("E", "Á"); ("F", "A̮")
      ("G", "A̋"); ("H", "A̰"); ("I", "Ả"); ("J", "A̓"); ("K", "Ạ"); ("L", "Ă")
      ("M", "Ǎ"); ("N", "Â"); ("O", "Å"); ("P", "A̯"); ("Q", "A̤"); ("R", "Ȃ")
      ("S", "Ã"); ("T", "Ā"); ("U", "Ä"); ("V", "À"); ("W", "Ȁ"); ("X", "A̽")
      ("Y", "A̦"); ("Z", "A̷") ]

let encode (unencoded: string) =
    let convert ch =
        let asStr = ch.ToString()
        let encodingMap = Map.ofList encodingPairs

        match encodingMap.TryGetValue asStr with
        | true, found -> found
        | false, _ -> asStr

    unencoded
    |> Seq.map convert
    |> String.concat String.Empty

let decode (encoded: string) =
    let decodingMap =
        // For detecting some additional characters that might be used instead of the expected ones.
        let extraDecodingPairs =
            [ ("A̸", "Z"); ("A̅", "T"); ("Ȧ", "B"); ("Á", "E"); ("Ả", "I"); ("Ạ", "K")
              ("Ă", "L"); ("Ǎ", "M"); ("Â", "N"); ("Å", "O"); ("Ȃ", "R"); ("Ã", "S")
              ("Ā", "T"); ("Ä", "U"); ("À", "V"); ("Ȁ", "W"); ("A̱", "D"); ("A̲", "D") ]

        encodingPairs
        |> List.map flip // Arrange encoded chars as keys.
        |> List.append extraDecodingPairs
        |> Map.ofList

    let convert text =
        match decodingMap.TryGetValue text with
        | true, decodedText -> decodedText
        | false, _ -> text

    let encodedStringInfo = StringInfo encoded

    // `SubstringByTextElements` is used to properly iterate over composed Unicode characters.
    let extractCharAt i : string = encodedStringInfo.SubstringByTextElements(i, 1)

    [| 0 .. encodedStringInfo.LengthInTextElements - 1 |]
    |> Array.map extractCharAt
    |> Array.map convert
    |> String.Concat

// Confirms that provided input is correctly encoded and decoded to its original value.
let test (unencodedText: string) =
    let encoded = encode unencodedText
    let decoded = decode encoded
    let result =
        if caseInsensitiveEquals unencodedText decoded
        then "OK"
        else "ERROR"

    $"%s{result}: %s{unencodedText} --> %s{encoded} --> %s{decoded}"
