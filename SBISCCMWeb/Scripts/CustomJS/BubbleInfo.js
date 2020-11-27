// Set Bubble info for the display help data
function triggerBubble() {
    $('.bubbleInfo').each(function () {

        var distance = 10;
        var time = 150;
        var hideDelay = 50;

        var hideDelayTimer = null;

        var beingShown = false;
        var shown = false;
        var trigger = $('.trigger', this);
        var info = $('.infoPopup', this).css('opacity', 1);

        $([trigger.get(0), info.get(0)]).mouseover(function () {
            if (hideDelayTimer) clearTimeout(hideDelayTimer);
            if (beingShown || shown) {
                // don't trigger the animation again
                return false;
            } else {

                // reset position of info box
                beingShown = true;

                info.css({
                    top: 40,
                    left: 0,
                    display: 'block'
                }).animate({
                    top: '-=' + distance + 'px',
                    opacity: 1
                }, time, 'swing', function () {
                    beingShown = false;
                    shown = true;
                });
            }

            return false;
        }).mouseout(function () {
            if (hideDelayTimer) clearTimeout(hideDelayTimer);
            hideDelayTimer = setTimeout(function () {
                hideDelayTimer = null;
                info.animate({
                    top: '-=' + distance + 'px',
                    opacity: 0
                }, time, 'swing', function () {
                    shown = false;
                    info.css('display', 'none');
                });

            }, hideDelay);

            return false;
        });
    });
}
$(function () {
    triggerBubble();
});