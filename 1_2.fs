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
  IF -2 nip nip nip nip
  ELSE 2drop 2dup = IF 2drop 0 ELSE < IF 1 ELSE -1 THEN THEN
  THEN
;

: is-digit 
  over c@ dup 10 =
  IF ( first character is \n ) drop -1 -2
  ELSE dup 48 >= over 57 <= and
       IF 48 - 1
       ELSE drop 2dup S" one" compare abs 1 = IF 1 2
       ELSE 2dup S" two" compare abs 1 = IF 2 2
       ELSE 2dup S" three" compare abs 1 = IF 3 4
       ELSE 2dup S" four" compare abs 1 = IF 4 4
       ELSE 2dup S" five" compare abs 1 = IF 5 3
       ELSE 2dup S" six" compare abs 1 = IF 6 3
       ELSE 2dup S" seven" compare abs 1 = IF 7 4
       ELSE 2dup S" eight" compare abs 1 = IF 8 4
       ELSE 2dup S" nine" compare abs 1 = IF 9 3
       ELSE 1 -1
       THEN THEN THEN THEN THEN THEN THEN THEN THEN THEN
  THEN
  2swap drop drop
;

variable sum

variable break 200000 break !

: print-if-break-gt-0 
break @ if dup dup . ." -> " sum @ + . CR break @ 1- break ! else then
;

: parse
  0 0
  BEGIN 2 pick 0> WHILE
    3 pick 3 pick is-digit case
    -2 of 0 2swap swap 10 * + print-if-break-gt-0 sum @ + sum ! 2drop 1- swap 1+ swap 0 0 endof
    -1 of 0 2rot 1- swap 1+ swap 2rot 2rot 2drop endof
    2swap drop 2 pick swap dup 0= IF drop 2 pick ELSE THEN swap 2swap
    2rot 2 pick - swap 2 pick + swap 2rot 2rot nip
    endcase
  REPEAT
;

\ s\" two1nine\neightwothree\nabcone2threexyz\nxtwone3four\n4nineeightseven2\nzoneight234\n7pqrstsixteen\n"
\ 0 sum ! parse sum @ CR . CR bye

: advcode1_1
  0 sum ! CR parse ;
create buffer 1000000 allot
: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input1_1" open-read buffer swap advcode1_1 sum @ CR . CR bye