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
create seeds 5000 cells allot

create maps 2000 cells allot

: parse ( addr u -- )
    6 - swap 6 + swap
    0 rot rot
    begin over C@ 10 <> while
        read-number 2swap drop
        3 pick cells seeds + !
        rot 1+ rot rot
        read-number 2swap drop
        3 pick 1- cells seeds + @ + 3 pick cells seeds + !
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
            read-number 2swap drop 3 pick 1- cells maps + @ + 3 pick cells maps + ! rot 1+ rot rot
        then
    repeat
    2drop
;

: 3dup { a b c } a b c a b c ;

: process-interval { sfrom sto iseed mend mpos }
    begin mpos mend < while
        mpos @
        dup -1 <> if
            mpos cell+ @ mpos 2 cells + @
            over sfrom > if
                \ start of seed < end of interval
                over sto >= if
                    \ end of seed <= end of interval
                    \ interval:            |-----------|
                    \ seed:       |-------|
                    2drop drop mpos 3 cells + to mpos
                else
                    \ end of seed > start of interval
                    dup sto >= if
                        \ end of seed <= end of interval
                        \ interval:        |-----------|
                        \ seed:       |---------|
                        over seeds nbseeds @ cells + !
                        sto seeds nbseeds @ 1+ cells + !
                        over seeds iseed 1+ cells + !
                        nbseeds @ dup 2 + nbseeds ! nip
                        over sto rot mend mpos recurse
                        nip to sto mpos 3 cells + to mpos
                    else
                        \ end of seed > end of interval
                        \ interval:        |-----------|
                        \ seed:       |--------------------|
                        over seeds nbseeds @ cells + !
                        dup seeds nbseeds @ 1+ cells + !
                        dup seeds nbseeds @ 2 + cells + !
                        sto seeds nbseeds @ 3 + cells + !
                        over seeds iseed 1+ cells + !
                        nbseeds @ dup 4 + nbseeds !
                        3dup mend mpos recurse
                        sto swap 2 + mend mpos recurse
                        nip to sto mpos 3 cells + to mpos
                    then
                then
            else
                \ start of seed >= start of interval
                dup sfrom <= if
                    \ start of seed >= end of interval
                    \ interval:    |-----------|
                    \ seed:                     |---------|
                    2drop drop mpos 3 cells + to mpos
                else
                    \ start of seed < end of interval
                    dup sto >= if
                        \ end of seed <= end of interval
                        \ interval:        |-----------|
                        \ seed:               |----|
                        drop - dup sto + to sto sfrom + to sfrom
                        sfrom seeds iseed cells + !
                        sto seeds iseed 1+ cells + !
                        begin mpos @ -1 <> while mpos cell+ to mpos repeat mpos cell+ to mpos
                    else
                        \ end of seed > end of interval
                        \ interval:        |-----------|
                        \ seed:                  |-----------|
                        dup seeds nbseeds @ cells + !
                        sto seeds nbseeds @ 1+ cells + !
                        dup seeds iseed 1+ cells + !
                        nbseeds @ dup 2 + nbseeds !
                        over sto rot mend mpos recurse
                        to sto - dup sto + to sto sfrom + to sfrom
                        sfrom seeds iseed cells + !
                        sto seeds iseed 1+ cells + !
                        begin mpos @ -1 <> while mpos cell+ to mpos repeat mpos cell+ to mpos
                    then
                then
            then
        else
            drop mpos cell+ to mpos
        then
    repeat
;

: advcode
    parse
    nbseeds @ 0 do
        seeds I cells + @
        seeds I 1+ cells + @
        I
        3 pick cells maps +
        maps
        process-interval
    2 +loop 
    seeds nbseeds @ min-array
;

create buffer 1000000 allot

: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input5" open-read buffer swap advcode CR . CR bye