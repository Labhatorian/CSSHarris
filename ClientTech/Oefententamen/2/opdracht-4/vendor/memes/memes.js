/**
* Een constructor function is ook toegestaan.
**/
class Meme {
    constructor(memeList) {
    this.cursor = 0;
    this.memeList = memeList;
    if(memeList.length === 0){
    throw new Error('De memelijst is leeg, verstrek een lijst met minimaal 1 meme.');
    }
    }

    getNextMeme(){
    if(this.cursor === this.memeList.length){
    this.cursor = 0;
    }
    return this.memeList[this.cursor++];
    }

    searchMeme(search){
    return this.memeList.filter(m => m.includes(search));
    }

}
