const pipes = [];

function apiCall(req) {
  return `Hello ${req}`;
}

function warp(req, next) {
  console.log(`start wrap ${req}`)
  const response = next(req);
  console.log(`end wrap ${response}`)
}

function tryWrap(req, next) {
  console.log(`trying ${req}`)
  return next(req)
}

class Pipe {
  constructor(action, next) {
    this.action = action;
    this.next = next;
  }
}

function addPipe(pipe) {
  if (pipes.length === 0) {
    pipes.push((req) => pipe(req, apiCall));
  } else {
    const previousPipe = pipes[pipes.length-1];
    pipes.push((req) => pipe(req, previousPipe));
  }
}

function build() {
  return pipes[pipes.length -1];
}

function main() {
  addPipe(tryWrap);
  addPipe(warp);

  const entry = build();
  entry("WORLD");
}

main();




