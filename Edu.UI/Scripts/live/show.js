console.log('show');

var player = videojs('example-video', { "poster": "", "controls": "true" },
    function () {
        this.on('play', function () {
            console.log('playing');
        });

        //暂停--播放完毕后也会暂停
        this.on('pause', function () {
            console.log("paused");
        });

        // 结束
        this.on('ended',
            function () {
                console.log('over');
            });

    });