const Mario = (function ($) {

        const stateMap = {
            score: 0,
            scoreSelector: null,
            marioSelector: null
        };

        const getNextScore = () => {
            return ++stateMap.score;
        };

        const setScore = () => {

            $(stateMap.scoreSelector).text(getNextScore());
        };

        const init = (marioSelector, scoreSelector, handleHit) => {

            stateMap.scoreSelector = scoreSelector;
            $(marioSelector).on('click', () => {
                setScore();
                handleHit();
            })

        };

        return {
            init: init
        }


})(jQuery);
