import { output, setOutput } from "../../util/output";
import { Expression } from "../Abstract/Expression";
import { Instruction } from "../Abstract/Instruction";
import { Environment } from "../Symbols/Environment";

export class Print extends Instruction {
  constructor(private value: Expression, line: number, column: number) {
    super(line, column);
  }

  public execute(environment: Environment) {
    const result = this.value.execute(environment);
    setOutput(`${output}\t${result.value}\n`);
    return result;
  }
}
