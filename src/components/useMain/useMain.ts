import { useState } from "react";
import { examples } from "../../util/examples";
import { parse } from "../../grammar/grammar";
import { Environment } from "../../interpreter/Symbols/Environment";
import { clearOutput, getOutput } from "../../util/output";

const useMain = () => {
  const [code, setCode] = useState("");
  const [result, setResult] = useState("Salida: ");
  const [actualExample, setActualExample] = useState(0);

  const parseInput = () => {
    clearOutput();
    const result = parse(code);
    const globalEnvironment = new Environment(null);
    for (const instruction of result) {
      instruction.execute(globalEnvironment);
    }
    setResult(`Salida:\n${getOutput()}`);
  };

  const selectExample = (index: string) => {
    setActualExample(parseInt(index));
    setCode(examples[parseInt(index)]);
  };

  return {
    code,
    setCode,
    result,
    parseInput,
    actualExample,
    selectExample,
  };
};

export default useMain;
