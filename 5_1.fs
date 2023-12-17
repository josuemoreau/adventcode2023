: read-number ( addr1 u1 -- d addr2 u2 )
    begin over C@ 32 = while 1- swap 1+ swap repeat
    0 0 2swap >number ;

: min-array ( addr u -- n )
    cells over + swap
    dup @ rot rot do
        I @ 2dup > if nip else drop then
    cell +loop
;

variable nbseeds 20 nbseeds !
create seeds nbseeds @ cells allot

create maps 2000 cells allot

: parse ( addr u -- )
    6 - swap 6 + swap
    0 rot rot
    begin over C@ 10 <> while
        read-number 2swap drop
        3 pick cells seeds + !
        rot 1+ rot rot
    repeat
    rot drop 0 rot 2 + rot 2 -
    begin dup 0> while
        begin over C@ 10 <> while 1- swap 1+ swap repeat 1- swap 1+ swap \ skip everything until next line
        dup 0<= if 1 else over C@ 10 = then if 
            \ end of map
            rot dup cells maps + -1 swap ! 1+ rot 1+ rot 1-
        else
            read-number 2swap drop 3 pick cells maps + ! rot 1+ rot rot
            read-number 2swap drop 3 pick cells maps + ! rot 1+ rot rot
            read-number 2swap drop 3 pick cells maps + ! rot 1+ rot rot
        then
    repeat
    2drop
;

: advcode
    parse CR

    nbseeds @ cells seeds + seeds do
        dup cells maps + maps
        ." Seed: " I @ . CR
        begin 2dup > while
            dup @
            dup -1 <> if
                over cell+ @
                2 pick 2 cells + @
                over + I @ > if dup I @ <= else 0 then if
                    I @ swap - + ." MAPPED TO " dup . CR I !
                    begin dup @ -1 <> while cell+ repeat cell+
                else
                    2drop 3 cells +
                then
            else drop cell+ ." UNCHANGED" CR
            then
        repeat
        2drop
        CR
    cell +loop
    drop seeds nbseeds @ min-array
;

create buffer 1000000 allot

: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input5" open-read buffer swap advcode CR . CR bye