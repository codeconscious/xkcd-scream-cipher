module Utilities

open System
open System.Collections.Generic

let flip map : Map<'a, 'b> =
    Map.fold (fun acc k v -> Map.add v k acc) Map.empty map

// Merge two maps with the values of the 'primary' one taking precedence.
let merge secondary primary : Map<'a, 'b> =
    Map.fold (fun acc k v -> Map.add k v acc) secondary primary

let caseInsensitiveEquals (x: string) (y: string) : bool =
    x.Equals(y, StringComparison.InvariantCultureIgnoreCase)

let groupByValues (items: KeyValuePair<'b, 'a> seq) : ('a * 'b seq) seq =
    items
    |> Seq.groupBy _.Value
    |> Seq.map (fun (groupValue, groupItems) ->
        let keys = groupItems |> Seq.map _.Key
        groupValue, keys)

let toNestedPairs (innerSeparator: string)
                  (outerSeparator: string)
                  (items: ('a * 'b seq) seq)
                  : string =
    items
    |> Seq.map (fun (_, v) -> String.Join(innerSeparator, v))
    |> String.concat outerSeparator
