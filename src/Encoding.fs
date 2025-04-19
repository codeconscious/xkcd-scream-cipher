module Encoding

open System
open System.Globalization
open Utilities

let private encodingMap =
    [ ("A", "A"); ("B", "Ȧ"); ("C", "A̧"); ("D", "A̠"); ("E", "Á"); ("F", "A̮")
      ("G", "A̋"); ("H", "A̰"); ("I", "Ả"); ("J", "A̓"); ("K", "Ạ"); ("L", "Ă")
      ("M", "Ǎ"); ("N", "Â"); ("O", "Å"); ("P", "A̯"); ("Q", "A̤"); ("R", "Ȃ")
      ("S", "Ã"); ("T", "Ā"); ("U", "Ä"); ("V", "À"); ("W", "Ȁ"); ("X", "A̽")
      ("Y", "A̦"); ("Z", "A̷") ]
    |> Map.ofList

let private decodingMap =
    // For detecting some additional characters that might be used instead of the expected ones.
    let extraPairs =
        [ ("A̸", "Z"); ("A̅", "T"); ("Ȧ", "B"); ("Á", "E"); ("Ả", "I"); ("Ạ", "K")
          ("Ă", "L"); ("Ǎ", "M"); ("Â", "N"); ("Å", "O"); ("Ȃ", "R"); ("Ã", "S")
          ("Ā", "T"); ("Ä", "U"); ("À", "V"); ("Ȁ", "W"); ("A̱", "D"); ("A̲", "D") ]
        |> Map.ofList

    encodingMap
    |> flip
    |> merge extraPairs

let encode (input: string) =
    let convert input =
        let inputAsStr = input.ToString()

        match encodingMap.TryGetValue inputAsStr with
        | true, encoded -> encoded
        | false, _ -> inputAsStr

    input
    |> Seq.map convert
    |> String.Concat

let decode (encodedText: string) =
    let convert encoded =
        match decodingMap.TryGetValue encoded with
        | true, decoded -> decoded
        | false, _ -> encoded

    let encodedStringInfo = StringInfo encodedText

    // `SubstringByTextElements` is used to properly iterate over composed Unicode characters.
    let extractCharAt i = encodedStringInfo.SubstringByTextElements(i, 1)

    [| 0 .. encodedStringInfo.LengthInTextElements - 1 |]
    |> Array.map extractCharAt
    |> Array.map convert
    |> String.Concat

// Confirms that provided input is correctly encoded and decoded to its original value.
let test (input: string) =
    let encoded = encode input
    let decoded = decode encoded
    let result =
        if caseInsensitiveEquals input decoded
        then "OK"
        else "ERROR"

    $"%s{result}: %s{input} --> %s{encoded} --> %s{decoded}"
