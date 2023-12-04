: not IF 0 ELSE -1 THEN ;

\ n = 0  if both strings are equal
\ n = -1 if second string starts with the first one
\ n = 1  if first string starts with the second one
\ n = -2 otherwise
: compare ( addr1 u1 addr2 u2 -- n )
  rot 2dup 2rot 2rot min 0 swap 0 do
    \ .s CR
    drop over I + C@ over I + C@ = IF 0 ELSE 1 LEAVE THEN
  loop
  IF -2
  ELSE 2drop 2dup = IF 0 ELSE < IF 1 ELSE -1 THEN THEN
  THEN
;

\ S" abcdef" S" abc" compare . CR bye

: is-digit 
  over c@ dup 10 =
  IF ( first character is \n ) -1 -2
  ELSE dup 48 >= over 57 <= and
       IF 48 - 1
       ELSE drop 2dup S" zero" compare abs 1 = IF 0 3
       ELSE 2dup S" one" compare abs 1 = IF 1 2
       ELSE 2dup S" two" compare abs 1 = IF 2 3
       ELSE 2dup S" three" compare abs 1 = IF 3 4
       ELSE 2dup S" four" compare abs 1 = IF 4 4
       ELSE 2dup S" five" compare abs 1 = IF 5 3
       ELSE 2dup S" six" compare abs 1 = IF 6 3
       ELSE 2dup S" seven" compare abs 1 = IF 7 4
       ELSE 2dup S" eight" compare abs 1 = IF 8 4
       ELSE 2dup S" nine" compare abs 1 = IF 9 3
       ELSE 1 -1
       THEN THEN THEN THEN THEN THEN THEN THEN THEN THEN THEN
  THEN
;

: test
  S" twoblablabla" .s CR is-digit .s CR case
  -2 of drop ." end of line -- +1" endof
  -1 of drop ." not found -- +1" endof
  swap ." digit " . dup ."  -- " .
  endcase CR .s CR
;

test
bye

variable sum

: parse
  CR
  0 0
  BEGIN 2 pick 0> WHILE
    3 pick 3 pick .s CR is-digit .s CR case
    -2 of 0 2swap swap 10 * + sum @ + sum ! 2rot 1- swap 1+ swap 2rot 2rot nip 0 0 rot endof
    -1 of 0 .s CR 2rot bye 3 pick - swap 3 pick + swap 2rot 2rot nip endof
    2swap dup 0= IF drop 2 pick ELSE THEN 2swap
    2rot 3 pick - swap 3 pick + swap 2rot 2rot nip
    endcase
  REPEAT
;

\ : first-last-digit
\   0 0 0 3 pick 0
\   DO     drop 3 pick i + 3 pick i - . CR c@ dup 10 = 
\          IF     drop i 1+ LEAVE 
\          ELSE   
\          THEN 
\          dup 48 >= over 57 <= and 
\          IF     2 pick 0= 
\                 IF     rot drop dup rot rot 
\                 ELSE   
\                 THEN 
\                 nip 
\          ELSE   drop 
\          THEN 
\          i' 
\   LOOP
\   ;
\ : parse-line first-last-digit 2rot 2 pick + 2rot swap 4 pick - swap 2rot drop 48 - swap 48 - 10 * + ;
\ : advcodeloop1_1  
\   parse-line sum @ + sum ! dup 0> 
\   IF     recurse
\   ELSE   
\   THEN ;
: advcode1_1  
  0 sum ! parse ;
create buffer 1000000 allot
: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input1_1" open-read buffer swap advcode1_1 sum @ CR . CR bye