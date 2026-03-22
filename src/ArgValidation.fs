module ArgValidation

open CCFSharpUtils.Library
open System
open System.Collections.Generic
open FsToolkit.ErrorHandling

type Operation = Encode | Decode | Test

type ValidatedArgs =
    { Operation: Operation
      Inputs: string array }

let supportedFlags =
    // Dictionaries, as currently implemented, preserve the order of added items,
    // whereas F#'s native Map does not.
    let flags = Dictionary<string, Operation>()
    flags.Add("--encode", Encode)
    flags.Add("-e", Encode)
    flags.Add("--decode", Decode)
    flags.Add("-d", Decode)
    flags.Add("--test", Test)
    flags.Add("-t", Test)
    flags

let flagSummary =
    let flags =
        supportedFlags
        |> KeyValuePair.groupByValues
        |> String.tupleValuesToNestedPairs ", " "; " // Formatting sample: "--encode, -e; --decode, -d"

    $"""Supported flags: %s{String.Join(", ", flags)}."""

let private validateArgCount (args: string array) =
    let instructions = $"Supply an operation flag and at least one string to convert.\n%s{flagSummary}"

    match args.Length with
    | 0 -> Error $"No arguments were passed. %s{instructions}"
    | 1 -> Error $"Insufficient arguments. %s{instructions}"
    | _ -> Ok args

let private validateFlag (flag: string) =
    if supportedFlags.ContainsKey flag
    then Ok supportedFlags[flag]
    else Error $"Unsupported flag \"%s{flag}\". %s{flagSummary}"

let validate (rawArgs: string array) =
    result {
        let! args = validateArgCount rawArgs
        let flag, inputs = args[0], args[1..]
        let! operation = validateFlag flag
        return { Operation = operation
                 Inputs = inputs |> Array.map _.ToUpperInvariant() }
    }

