: compare ( addr1 u1 addr2 u2 -- n )
  rot 2dup 2rot 2rot min 0 swap 0 do
    drop over I + C@ over I + C@ = IF 0 ELSE 1 LEAVE THEN
  loop
  IF -2 nip nip nip nip
  ELSE 2drop 2dup = IF 2drop 0 ELSE < IF 1 ELSE -1 THEN THEN
  THEN
;

variable sum

: parse
    begin 5 - swap 5 + swap dup 0> while
        0 0 2swap >number
        1
        begin 2 pick C@ 10 <> over and while
            drop
            0 0 2swap 2 - swap 2 + swap >number 1- swap 1+ swap
            2dup S" blue" compare abs 1 = IF 2swap drop 14 > IF 0 ELSE 4 - swap 4 + swap 1 THEN 
            ELSE 2dup S" red" compare abs 1 = IF 2swap drop 12 > IF 0 ELSE 3 - swap 3 + swap 1 THEN
            ELSE 2dup S" green" compare abs 1 = IF 2swap drop 13 > IF 0 ELSE 5 - swap 5 + swap 1 THEN
            ELSE .s bye
            THEN THEN THEN
        repeat
        IF 2swap drop sum @ + sum ! ELSE 2nip THEN
        begin over C@ 10 <> while 1- swap 1+ swap repeat
        1- swap 1+ swap
    repeat
;

: advcode 0 sum ! parse ;


create buffer 1000000 allot
: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input2_1" open-read buffer swap advcode sum @ CR . CR bye