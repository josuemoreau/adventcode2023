variable len
variable sum

: check-number ( addr1 addr2 u -- n ) { addr1 addr2 pos -- n }
  pos len @ mod if
    addr1 1- C@ '.' <> addr1 1- len @ - C@ '.' <> or addr1 1- len @ + C@ '.' <> or
  else 0
  then

  0= if
    addr2 addr1 - pos 1+ + len @ mod if
        addr2 C@ '.' <> addr2 len @ - C@ '.' <> or addr2 len @ + C@ '.' <> or
    else 0
    then
  else 1 then

  0= if
    0 addr2 addr1 do
        drop I len @ + C@ '.' = I len @ - C@ '.' = and if 0 else 1 leave then
    loop
  else 1 then
;

: read-number ( addr1 u1 n1 -- addr2 u2 n2 n ) { addr1 u1 n1 -- addr2 u2 n2 n }
  0 0 addr1 u1 >number ( u u addr2 u2 )
  over addr1 swap n1 ( u u addr2 u2 addr1 addr2 n1 ) check-number
  if 2swap drop over u1 swap - n1 + swap
  else 2nip dup u1 swap - n1 + -1
  then
;

: parse ( addr u -- )
  0 begin over 0> while
    2 pick C@ dup dup 48 >= swap 57 <= and if
      drop read-number dup 0> if
        ." Found " dup .
        sum @ + sum !
        ."  -> " sum @ . .s CR
      else drop
      then
    else 10 = if 
      drop 0 rot 1+ rot 1- rot
    else
      1+ rot 1+ rot 1- rot
    then then
  repeat
  2drop drop
;

create buffer 1000000 allot

: get-line-length ( x name -- x n )
  R/O open-file drop dup dup file-size drop drop buffer swap rot read-line 2drop swap close-file drop ; 

: fill ( buf c len -- )
  0 do dup 2 pick I + ! loop 2drop ;

: open-read ( x name u buf -- x n ) { name u buf }
  name u R/O open-file drop dup dup file-size drop drop buf swap rot read-file drop swap close-file drop
;

: advcode 0 sum ! 
  2dup get-line-length len ! \ get line length
  buffer '.' len @ fill \ fill a first line with dots
  len @ buffer + dup 10 swap ! 1+ \ add \n and shift pointer to buffer
  rot rot 2 pick open-read \ read the file
  swap over + '.' len @ fill \ fill last line with dots
  len @ 1+ len ! \ increment len because of \n at end of lines
  CR buffer swap len @ 2* +
  parse sum @
;

0 S" input3" advcode . CR bye