
import axios from 'axios';

const apiUrl = "https://localhost:7271"
axios.defaults.baseURL = "http://localhost:5083/";


export default {

  getTasks: async () => {
    try {
      const response = await axios.get(`${apiUrl}/items`);
      return response.data;
    } catch (error) {
      console.error("Error fetching tasks:", error);
      throw error; // Re-throw the error to propagate it further if needed
    }
  },
  addTask: async (name) => {
    try {
      console.log('addTask', name);
      const result = await axios.post(`${apiUrl}/items/${name}`);
      return result.data;
    } catch (error) {
      console.error("Error adding task:", error);
      throw error; // Re-throw the error to propagate it further if needed
    }
  },

  setCompleted: async (id, isComplete) => {
    try {
      console.log('setCompleted', { id, isComplete });
      const result = await axios.put(`${apiUrl}/items/${id}`);
      // TODO: Handle completion logic if needed
      return result.data;
    } catch (error) {
      console.error("Error setting task completion status:", error);
      throw error; // Re-throw the error to propagate it further if needed
    }
  },

  deleteTask: async (id) => {
    try {
      console.log('deleteTask', id);
      const result = await axios.delete(`${apiUrl}/items/${id}`);
      return result.data;
    } catch (error) {
      console.error("Error deleting task:", error);
      throw error; // Re-throw the error to propagate it further if needed
    }
  }
};





