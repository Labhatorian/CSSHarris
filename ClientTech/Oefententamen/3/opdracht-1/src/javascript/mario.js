const SuperMario = (() => {

    const jump = async (height) => {

        return new Promise(resolve => {
            if(!height){
                console.log('normal jump');
            }else{
                console.log(`${height} jump`);
            }
            resolve(height);
        })

    }

    const jumping = async () => {
        for (let i = 0; i < 3; i++) {
            await jump();
        }
    }

    return {jump: jump, jumping: jumping}
})();

