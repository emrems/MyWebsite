import { createStore } from 'vuex';
import projects from './modules/projects';
import articles from './modules/articles';
import experience from './modules/experience';
import comments from './modules/comments';


export default createStore({
  modules: {
    projects, 
    articles,
    experience,
    comments
  }
});