<script setup lang="ts">
  import { storeToRefs } from 'pinia';
  import {onMounted, ref} from 'vue'
  import { useAirportStore } from '@/stores/counter';

  const fruitValue = ref('');

  const fruits = ref<Array<string>>([]);

  const airportStore = useAirportStore();
  const { getArrivals} = airportStore;
  const { arrivals } = storeToRefs(airportStore);

  function addFruit(fruit: string){
    fruits.value.push(fruit);
  }

  onMounted(()=> {
    getArrivals();
  })
</script>

<template>
  <main>
    <input type="text" v-model="fruitValue" @keyup.enter="addFruit(fruitValue)" />

    <button @click="getArrivals"> Haal aankomsten op</button>

    <ul>
      <li v-for="arrival in arrivals">
          {{ arrival }}
      </li>
    </ul>
  </main>
</template>

<style>
 .text-white {
  color: aliceblue;
 }
</style>