import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { retrieveArrivals } from '@/api'

const beginTimestamp = 1616454000; // 23 maart 2021 00:00:00
const endTimestamp = 1616540400; // 24 2021 maart 00:00:00

export const useAirportStore = defineStore('airport', () => {
  const airport = ref<string>('');
  const arrivals = ref<Array<any>>([]);

  function setAirport(name: string) {
    // Ken de waarde name toe aan 'airport'
  }

  async function getArrivals() {
    const response = await retrieveArrivals('EHAM', beginTimestamp, endTimestamp);
    arrivals.value = response.data;

    console.log('response', response)
  }

  return {
    airport,
    arrivals,
    setAirport,
    getArrivals
  }
})
