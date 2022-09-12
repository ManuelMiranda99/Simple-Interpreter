export let output = "";

export const clearOutput = () => {
  output = "";
};

export const getOutput = () => {
  return output;
};

export const setOutput = (newOutput: string) => {
  output = newOutput;
};
