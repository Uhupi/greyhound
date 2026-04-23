<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import type { ClockState } from './types/ClockState';
    import { ApiService } from './services/ApiService';
    import { minutesTopColor } from './utils/clockUtils';

    let state: ClockState = {
        seconds: false,
        hoursTop: [false, false, false, false],
        hoursBottom: [false, false, false, false],
        minutesTop: new Array(11).fill(false),
        minutesBottom: [false, false, false, false],
    };

    let error: boolean = false;
    let interval: ReturnType<typeof setInterval>;

    async function fetchTimeData() {
    try {
        state = await ApiService.fetchFromUrl();
        error = false;
    } catch {
        error = true;
    }
  }

  onMount(() => {
    fetchTimeData();
    interval = setInterval(fetchTimeData, 1000);
  });

  onDestroy(() => clearInterval(interval));
</script>

<h1>Mengenlehreuhr</h1>

{#if error}
<p class="error">Cannot reach backend at {ApiService.CLOCK_API_URL}</p>
{/if}

<!-- Seconds blink -->
<div class="row">
<div class="lamp circle" class:lit-yellow={state.seconds}></div>
</div>

<!-- Row 1: 5-hour lamps (4x red) -->
<div class="row">
{#each state.hoursTop as lit}
    <div class="lamp rect" class:lit-red={lit}></div>
{/each}
</div>

<!-- Row 2: 1-hour lamps (4x red) -->
<div class="row">
{#each state.hoursBottom as lit}
    <div class="lamp rect" class:lit-red={lit}></div>
{/each}
</div>

<!-- Row 3: 5-minute lamps (11x, 3rd/6th/9th = red, rest yellow) -->
<div class="row">
{#each state.minutesTop as lit, i}
    <div
    class="lamp rect narrow"
    style="background-color: {minutesTopColor(i, lit)}"
    ></div>
{/each}
</div>

<!-- Row 4: 1-minute lamps (4x yellow) -->
<div class="row">
{#each state.minutesBottom as lit}
    <div class="lamp rect" class:lit-yellow={lit}></div>
{/each}
</div>

<style>
  .error {
    color: #f66;
    font-size: 0.9rem;
    margin-bottom: 0.5rem;
  }

  .row {
    display: flex;
    gap: 4px;
    align-items: center;
    justify-content: space-evenly;
    max-width: 320px;
    width: 100%;
  }

  .lamp {
    background-color: #2a2a2a;
    border: 2px solid #444;
    transition: background-color 0.15s;
  }

  .lamp.circle {
    width: 64px;
    aspect-ratio: 1;
    border-radius: 50%;
  }

  .lamp.rect {
    flex: 1;
    height: 44px;
    border-radius: 5px;
  }

  .lit-red {
    background-color: #cc2200 !important;
  }

  .lit-yellow {
    background-color: #ffd700 !important;
  }
</style>
