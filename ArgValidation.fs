module ArgValidation

open System
open FsToolkit.ErrorHandling

type Operation = Encode | Decode | Test

type ValidatedArgs =
    { Operation: Operation
      Inputs: string array }

// Abbreviated flags are currently unsupported due to an obscure bug involving "-d".
// (See https://github.com/dotnet/fsharp/issues/10819 for more.)
let supportedFlags = Map.ofList [
        "--encode", Encode
        "--decode", Decode
        "--test",   Test
    ]

let private validateArgCount (args: string array) =
    let instructions =
        sprintf
            "Supply an operation flag and at least one string to convert.\nSupported flags: %s"
            (String.Join(", ", supportedFlags.Keys))

    match args.Length with
    | 0 -> Error $"No arguments were passed. {instructions}"
    | 1 -> Error $"Insufficient arguments. {instructions}"
    | _ -> Ok args

let private validateFlag flag =
    if supportedFlags.ContainsKey flag
    then Ok supportedFlags[flag]
    else
        let flagSummary = String.Join(", ", supportedFlags.Keys)
        Error $"Unsupported flag \"%s{flag}\". You must use one of the following: {flagSummary}."

let private validateInputs (inputs: string array) =
    if inputs.Length = 0
    then Error "No inputs to convert were passed."
    else Ok (inputs |> Array.map _.ToUpperInvariant())

let validate (rawArgs: string array) =
    result {
        let! args = validateArgCount rawArgs
        let flag, inputs = args[0], args[1..]
        let! operation = validateFlag flag
        let! inputs' = validateInputs inputs
        return { Operation = operation; Inputs = inputs' }
    }

