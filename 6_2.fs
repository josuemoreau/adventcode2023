: read-number ( addr1 u1 -- d addr2 u2 )
    begin over C@ 32 = while 1- swap 1+ swap repeat
    0 0 2swap >number ;

variable time 0 time !
variable dist 0 dist !

: log10ceil ( n1 -- n2 )
    0 >R
    begin dup 0> while
        10 / R> 1+ >R
    repeat
    drop R>
;

: pow { x n } 1 n 0 do x * loop ;

: parse ( addr u -- )
    5 - swap 5 + swap
    begin over C@ 10 <> while
        read-number 2swap drop dup log10ceil 10 swap pow time @ * + time !
    repeat
    10 - swap 10 + swap
    begin over C@ 10 <> while
        read-number 2swap drop dup log10ceil 10 swap pow dist @ * + dist !
    repeat
    2drop
;

: f2dup fover fover ;

: advcode
    parse
    time @ . dist @ . CR
    time @ { t }
    dist @ { d }
    t 0 D>F fdup 0 0 D>F fswap f- fswap fdup f* d 0 D>F 4 0 D>F f* f- fsqrt
    f2dup
    f+ -2 S>F f/
    frot frot
    f- -2 S>F f/
    F>S F>S - .s CR
;

create buffer 1000000 allot

: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input6" open-read buffer swap advcode CR . CR bye