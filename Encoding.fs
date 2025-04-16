module Encoding

open System
open System.Globalization

let private encodingPairs =
    [ ("A", "A"); ("B", "Ȧ"); ("C", "A̧"); ("D", "A̠"); ("E", "Á"); ("F", "A̮")
      ("G", "A̋"); ("H", "A̰"); ("I", "Ả"); ("J", "A̓"); ("K", "Ạ"); ("L", "Ă")
      ("M", "Ǎ"); ("N", "Â"); ("O", "Å"); ("P", "A̯"); ("Q", "A̤"); ("R", "Ȃ")
      ("S", "Ã"); ("T", "Ā"); ("U", "Ä"); ("V", "À"); ("W", "Ȁ"); ("X", "A̽")
      ("Y", "A̦"); ("Z", "A̷") ]

let encode (unencodedText: string) =
    let convert ch =
        let asStr = ch.ToString()
        let encodingMap = encodingPairs |> Map.ofList

        match encodingMap.TryGetValue asStr with
        | true, found -> found
        | false, _ -> asStr

    unencodedText
    |> Seq.map convert
    |> String.concat String.Empty

let decode (encodedText: string) =
    let decodingMap =
        // For detecting some additional characters that might be used instead of the expected ones.
        let extraDecodingPairs = [
            "A̸", "Z"
            "A̅", "T"
            "Ȧ", "B"
            "Á", "E"
            "Ả", "I"
            "Ạ", "K"
            "Ă", "L"
            "Ǎ", "M"
            "Â", "N"
            "Å", "O"
            "Ȃ", "R"
            "Ã", "S"
            "Ā", "T"
            "Ä", "U"
            "À", "V"
            "Ȁ", "W"
            "A̱", "D"
            "A̲", "D"
        ]

        encodingPairs
        |> List.map (fun (x, y) -> y, x) // Ensure encoded chars are first.
        |> List.append extraDecodingPairs
        |> Map.ofList

    let convert text =
        match decodingMap.TryGetValue text with
        | true, found -> found
        | false, _ -> text

    let stringInfo = StringInfo encodedText

    [| 0 .. stringInfo.LengthInTextElements - 1 |]
    |> Array.map (fun i -> stringInfo.SubstringByTextElements(i, 1))
    |> Array.map convert
    |> String.Concat

// Confirms that provided input is correctly encoded and decoded to its original value.
let test originalText =
    let encodedText = encode originalText
    let decodedText = decode encodedText
    let result =
        if originalText.Equals(decodedText, StringComparison.InvariantCultureIgnoreCase)
        then "OK"
        else "ERROR"

    $"%s{result}: %s{originalText} --> %s{encodedText} --> %s{decodedText}"
