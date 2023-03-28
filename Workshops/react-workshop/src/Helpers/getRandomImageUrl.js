/**
 * @param {Object} options 
 * @returns {string}
 */
const getRandomImageUrl = (options) => {
    const {
        height = 200,
        seed = 'placeholder',
        width = 300,
    } = options || {};
    return `https://picsum.photos/seed/${seed}/${width}/${height}`
};

export default getRandomImageUrl;