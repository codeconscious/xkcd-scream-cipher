module Encoding

open CCFSharpUtils.Library
open System
open System.Globalization

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
    |> Map.flip
    |> Map.merge extraPairs

let encode inputText =
    let convert inputChar =
        encodingMap |> Map.valueOrTarget (inputChar.ToString())

    inputText
    |> Seq.map convert
    |> String.Concat

let decode encodedText =
    let encodedStringInfo = StringInfo encodedText

    // `SubstringByTextElements` is used to properly iterate over composed Unicode characters.
    let extractChar index = encodedStringInfo.SubstringByTextElements(index, 1)

    let convert encodedChar =
        decodingMap |> Map.valueOrTarget encodedChar

    [| 0 .. encodedStringInfo.LengthInTextElements - 1 |]
    |> Array.map (extractChar >> convert)
    |> String.Concat

// Confirms that provided input is correctly encoded and decoded to its original value.
let test inputText =
    let encoded = encode inputText
    let decoded = decode encoded

    let result =
        if String.equalIgnoreCase inputText decoded
        then "OK"
        else "ERROR"

    $"%s{result}: %s{inputText} --> %s{encoded} --> %s{decoded}"
