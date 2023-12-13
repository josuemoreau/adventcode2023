require insertion-sort.fs

: read-number ( addr1 u1 -- d addr2 u2 )
    begin over C@ 32 = while 1- swap 1+ swap repeat
    0 0 2swap >number ;

variable sum

create card 10 cells allot
create winning 25 cells allot

: compute-points
    0 0 0 begin over 10 < over 25 < and while
        over cells card + @ over cells winning + @
        2dup < if 2drop swap 1+ swap
        else > if 1+
        else 
        rot dup 0> if 1 LSHIFT else drop 1 then rot 1+ rot 1+
        then then
    repeat
    2drop
;

: parse ( addr u -- )
    begin dup 0> while
        9 - swap 9 + swap
        10 0 do
            read-number 2swap drop card I cells + !
        loop
        2 - swap 2 + swap
        25 0 do
            read-number 2swap drop winning I cells + !
        loop
        card 10 1 cells insertion-sort
        winning 25 1 cells insertion-sort
        compute-points sum @ + sum !
        1- swap 1+ swap
    repeat
;

: advcode 0 sum ! parse sum @ ;

create buffer 1000000 allot

: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input4" open-read buffer swap advcode CR . CR bye
