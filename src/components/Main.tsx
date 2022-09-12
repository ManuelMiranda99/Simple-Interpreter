import { FC } from "react";
import useMain from "./useMain/useMain";
import AceEditor from "react-ace";

import "ace-builds/src-noconflict/mode-python";
import "ace-builds/src-noconflict/mode-plain_text";
import "ace-builds/src-noconflict/theme-chaos";
import "ace-builds/src-noconflict/theme-terminal";

const Main: FC = () => {
  const { actualExample, code, parseInput, result, setCode, selectExample } =
    useMain();

  return (
    <div className="w-4/5 pt-6 m-auto my-6">
      <div className="relative">
        <select
          value={actualExample}
          onChange={(e) => selectExample(e.target.value)}
          className="select"
        >
          <option value={0}>Vacío</option>
          <option value={1}>If</option>
          <option value={2}>While</option>
        </select>
        <div className="absolute inset-y-0 right-0 flex items-center px-2 text-gray-700 pointer-events-none">
          <svg
            className="w-4 h-4 fill-current"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 20 20"
          >
            <path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" />
          </svg>
        </div>
      </div>
      <div className="relative my-6">
        <AceEditor
          mode="python"
          theme="chaos"
          fontSize={18}
          showPrintMargin={false}
          value={code}
          onChange={setCode}
          className="w-full h-full"
          width="100%"
        />
        <button className="absolute top-0 right-0 btn" onClick={parseInput}>
          Ejecutar
        </button>
      </div>
      <AceEditor
        mode="plain_text"
        theme="terminal"
        fontSize={18}
        showGutter={false}
        showPrintMargin={false}
        highlightActiveLine={false}
        readOnly={true}
        value={result}
        className="w-full h-full text-green-500"
        width="100%"
        height="200px"
      />
    </div>
  );
};

export default Main;
