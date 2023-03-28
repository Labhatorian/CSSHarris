# Steps
1. Create a 'Card' Component to replace the repetitive data from `GalleryContainer/index.jsx` (and give it a `onClick` event)
2. Fetch our planets from API in the `usePlanets.js` query
3. Ensure that every planet has an id property in `usePlanets.js`
4. Use iteration to render the Card component based on API data in `GalleryContainer/index.jsx`
5. Add a route for Details page in `App.jsx`, for example `/details/:planetId`
6. Navigate to Details page when the user clicks a card in `GalleryContainer/index.jsx`
7. Fetch the chosen planet on the Details page using `usePlanet.js`
8. Fill the Details page with the fetched data in `DetailContainer.js`

## Bonus
9. Make the tabs on the Details page work
10. Add a ‘previous’ button
11. Show a Loading indicator when fetching the data
12. Turn tabs into separate component
13. Turn info-table into separate component
14. Turn button into separate component
15. Create content for the ‘people’ tab



# Getting Started with Create React App

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in your browser.

The page will reload when you make changes.\
You may also see any lint errors in the console.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

### `npm run eject`

**Note: this is a one-way operation. Once you `eject`, you can't go back!**

If you aren't satisfied with the build tool and configuration choices, you can `eject` at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except `eject` will still work, but they will point to the copied scripts so you can tweak them. At this point you're on your own.

You don't have to ever use `eject`. The curated feature set is suitable for small and middle deployments, and you shouldn't feel obligated to use this feature. However we understand that this tool wouldn't be useful if you couldn't customize it when you are ready for it.

## Learn More

To learn React, check out the [React documentation](https://reactjs.org/).

### `npm run build` fails to minify

This section has moved here: [https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify](https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify)
