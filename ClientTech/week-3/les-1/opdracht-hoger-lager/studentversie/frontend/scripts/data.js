class Data {

    async getGetal(gok) {

//student uitwerking
        //naar server
        let r = await fetch("http://localhost:3000/getal?gok=" + gok)
        return r.json();
    }

}

export {Data};
