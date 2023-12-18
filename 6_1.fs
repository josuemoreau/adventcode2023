: read-number ( addr1 u1 -- d addr2 u2 )
    begin over C@ 32 = while 1- swap 1+ swap repeat
    0 0 2swap >number ;

variable cols 4 cols !
create time cols @ cells allot
create dist cols @ cells allot

: parse ( addr u -- )
    0 >R
    5 - swap 5 + swap
    begin over C@ 10 <> while
        read-number 2swap drop R@ cells time + !
        R> 1+ >R
    repeat
    R> drop 0 >R
    10 - swap 10 + swap
    .s CR
    begin over C@ 10 <> while
        read-number 2swap drop R@ cells dist + !
        R> 1+ >R
    repeat
    R> drop
    2drop
;

: f2dup fover fover ;

: advcode
    parse
    1
    cols @ 0 do
        time I cells + @ { t }
        dist I cells + @ { d }
        t 0 D>F fdup 0 0 D>F fswap f- fswap fdup f* d 0 D>F 4 0 D>F f* f- fsqrt
        f2dup
        f+ -2 S>F f/
        frot frot
        f- -2 S>F f/
        F>S F>S - .s * CR
    loop
;

create buffer 1000000 allot

: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input6" open-read buffer swap advcode CR . CR bye