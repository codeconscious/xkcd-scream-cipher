open ArgValidation
open Encoding

let getConverter = function
    | Encode -> encode
    | Decode -> decode
    | Test -> test

let convert args =
    let converter = getConverter args.Operation

    args.Inputs
    |> Array.map converter
    |> Array.iter (fun x -> printfn $"%s{x}")

[<EntryPoint>]
let main args =
    match validate args with
    | Ok validatedArgs ->
        convert validatedArgs
        0
    | Error e ->
        printfn $"Error: %s{e}"
        1
