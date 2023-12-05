: compare ( addr1 u1 addr2 u2 -- n )
  rot 2dup 2rot 2rot min 0 swap 0 do
    drop over I + C@ over I + C@ = IF 0 ELSE 1 LEAVE THEN
  loop
  IF -2 nip nip nip nip
  ELSE 2drop 2dup = IF 2drop 0 ELSE < IF 1 ELSE -1 THEN THEN
  THEN
;

variable sum
variable max_blue
variable max_red
variable max_green

: power max_blue @ max_red @ max_green @ * * ;

: parse
    begin 5 - swap 5 + swap ( skip `Game ` ) dup 0> ( and test if length > 0 ) while
        0 0 2swap >number \ read game number
        0 max_blue ! 0 max_red ! 0 max_green !
        begin 1 pick C@ 10 <> while \ while the next character is not \n
            0 0 2swap 2 - swap 2 + swap >number 1- swap 1+ swap \ skip `: `, `, ` or `; ` and read the next number
            \ then check the color
            2dup S" blue" compare abs 1 = IF \ get the read number and compare it with max_blue
                                             2swap drop dup max_blue @ > IF max_blue ! ELSE drop THEN
                                             4 - swap 4 + swap \ skip the word `blue`
            ELSE 2dup S" red" compare abs 1 = IF 2swap drop dup max_red @ > IF max_red ! ELSE drop THEN
                                                 3 - swap 3 + swap
            ELSE 2dup S" green" compare abs 1 = IF 2swap drop dup max_green @ > IF max_green ! ELSE drop THEN
                                                   5 - swap 5 + swap
            ELSE .s bye
            THEN THEN THEN
        repeat
        2nip \ remove the game number, not used in this second part of problem 2
        power sum @ + sum ! \ increase sum
        1- swap 1+ swap \ skip `\n`
    repeat
;

: advcode 0 sum ! parse ;


create buffer 1000000 allot
: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input2_1" open-read buffer swap advcode sum @ CR . CR bye