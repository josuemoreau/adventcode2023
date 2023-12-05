: compare ( addr1 u1 addr2 u2 -- n )
  rot 2dup 2rot 2rot min 0 swap 0 do
    drop over I + C@ over I + C@ = IF 0 ELSE 1 LEAVE THEN
  loop
  IF -2 nip nip nip nip
  ELSE 2drop 2dup = IF 2drop 0 ELSE < IF 1 ELSE -1 THEN THEN
  THEN
;

variable len
variable sum

: check-number ( addr1 addr2 u -- n ) { addr1 addr2 pos -- n }
  pos len @ mod if
    addr1 1- C@ '.' <> addr1 1- len @ - C@ '.' <> or addr1 1- len @ + C@ '.' <> or
  else 0
  then

  0= if
    addr2 addr1 - pos + len @ mod if
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
  over addr1 swap n1 ( u u addr2 u2 addr1 addr2 n1 ) check-number CR .s
  if 2swap drop over u1 swap - n1 + swap
  else 2nip dup u1 swap - n1 + -1
  then
;

: parse ( addr u -- )
  0 begin over 0> while
    2 pick C@ dup dup 48 >= swap 57 <= and if
      drop 1+ rot 1+ rot 1- rot
    else 10 = if 
      drop 0 rot 1+ rot 1- rot
    else
      1+ rot 1+ rot 1- rot
    then then
  repeat
\   2drop drop
;

\ create buffer 1000000 allot
6 len !
s\" .....\n.123.\n....." 7 - swap 7 + swap 1 read-number CR .s CR bye