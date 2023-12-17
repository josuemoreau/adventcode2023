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
create seeds 1000 cells allot
\ create seed-soil 200 cells allot
\ create soil-fertilizer 200 cells allot
\ create fertilizer-water 200 cells allot
\ create water-light 200 cells allot
\ create light-temperature 200 cells allot
\ create temperature-humidity 200 cells allot
\ create humidity-location 200 cells allot

create maps 2000 cells allot

: parse ( addr u -- )
    6 - swap 6 + swap
    0 rot rot
    \ rot 6 + rot 6 - rot
    begin over C@ 10 <> while
        read-number 2swap drop
        3 pick cells seeds + !
        rot 1+ rot rot
    repeat
    rot drop 0 rot 2 + rot 2 -
    begin dup 0> while
        begin over C@ 10 <> while 1- swap 1+ swap repeat 1- swap 1+ swap \ skip everything until next line
        \ 2dup read-number 2swap 2drop drop 2 pick = if 
        dup 0<= if 1 else over C@ 10 = then if 
            \ end of map
            rot dup cells maps + -1 swap ! 1+ rot 1+ rot 1-
        else
            \ 2dup drop 20 type CR
            read-number 2swap drop 3 pick cells maps + ! rot 1+ rot rot
            read-number 2swap drop 3 pick cells maps + ! rot 1+ rot rot
            read-number 2swap drop 3 pick cells maps + ! rot 1+ rot rot
        then
        \ 1- swap 1+ swap \ skip \n
        \ test end of map
    repeat
    2drop
;

: advcode
    parse CR

    .s CR

    seeds >R
    
    begin R@ nbseeds @ cells seeds + < while
    \ nbseeds @ cells seeds + seeds do
        dup cells maps + maps
        ." Seed: " R@ @ dup . ." -> " . ." + " R@ cell+ @ . CR
        begin 2dup > while
            dup @
            dup -1 <> if
                over cell+ @
                2 pick 2 cells + @
                ." -> " .s CR
                over +
                dup R@ @ > if
                    over R@ @ <= if
                        ( dest src_begin src_end )
                        \ .s CR
                        R@ @ R@ cell+ @ + over < if
                            ." INTERVAL IS ENTIRELY INCLUDED" CR
                            \ l'intervalle tient entièrement
                            drop R@ @ swap - + ." FULLY MAPPED TO " dup . ." + " R@ cell+ @ . CR R@ !
                        else
                            ." INTERVAL'S END NEEDS TO BE CUT      ===================================================================" CR
                            \ .s CR
                            R@ @ R@ cell+ @ + over - seeds nbseeds @ 1+ cells + ! dup seeds nbseeds @ cells + ! nbseeds @ 2 + nbseeds !
                            ." ADDED ONE SEED" CR
                            ( dest src_begin src_end )
                            rot rot ( src_end dest src_begin )
                            R@ @ swap - + swap R@ @ - R@ cell+ ! R@ !
                            
                            \  R@ cell+ @ R@ @ .s - .s CR swap R@ ! R@ cell+ !

                            nbseeds @ cells seeds + seeds do
                                I @ . ." "
                            cell +loop
                            CR CR

                            \ bye

                        then
                        begin dup @ -1 <> while cell+ repeat cell+
                    else
                        R@ cell+ @ R@ @ +
                        2 pick over < if
                            2dup >= if
                                nip
                                ." INTERVAL'S BEGINING NEEDS TO BE CUT ++++++++++++++++++++++++++++++++++++++++++++++" CR
                                ( dest src_begin seed_end )
                                2dup swap - seeds nbseeds @ 1+ cells + ! seeds nbseeds @ cells + ! nbseeds @ 2 + nbseeds !
                                ." ADDED ONE SEED" CR
                                ( dest src_begin )
                                R@ @ - R@ cell+ ! drop

                                nbseeds @ cells seeds + seeds do
                                    I @ . ." "
                                cell +loop
                                CR CR

                                begin dup @ -1 <> while cell+ repeat cell+
                            else
                                ." INTERVAL'S BEGINNING AND END NEED TO BE CUT --------------------------------------------------------" CR
                                .s CR
                                ( dest src_begin src_end seed_end )
                                2dup swap - seeds nbseeds @ 1+ cells + ! over seeds nbseeds @ cells + ! nbseeds @ 2 + nbseeds !
                                drop R@ @- 
                                
                                
                                 over R@ @ - R@ cell+ !
                                swap - 


                                drop 2dup over swap - seeds nbseeds @ 1+ cells + ! seeds nbseeds @ cells + ! nbseeds @ 2 + nbseeds !
                                drop R@ @ - cell+ ! drop

                                drop 2drop 3 cells +
                            then
                        else
                            2drop 2drop 3 cells +
                        then
                    then
                else
                    2drop drop 3 cells +
                then
            else drop cell+ ." UNCHANGED" CR
            then
        repeat
        2drop
        CR
        ." END OF SEED" CR
        R> 2 cells + >R
    repeat

    R> drop

    drop 
    
    CR
    nbseeds @ cells seeds + seeds do
        I @ . ." "
    cell +loop
    
    CR
    
    seeds nbseeds @ min-array
;

create buffer 1000000 allot

: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input5" open-read buffer swap advcode CR . CR bye