module Utilities

open System

let flipMap map : Map<'a, 'b> =
    Map.fold (fun acc k v -> Map.add v k acc) Map.empty map

// Merge two maps with the values of the 'primary' one taking precedence.
let mergeMaps secondary primary : Map<'a, 'b> =
    Map.fold (fun acc k v -> Map.add k v acc) secondary primary

let caseInsensitiveEquals (x: string) (y: string) : bool =
    x.Equals(y, StringComparison.InvariantCultureIgnoreCase)

