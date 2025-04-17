module Utilities

open System

let flip (x, y) = y, x

let caseInsensitiveEquals (x: string) (y: string) : bool =
    x.Equals(y, StringComparison.InvariantCultureIgnoreCase)
